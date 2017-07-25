# Patternizer

A little library for resolving patterns in a list of lines using a fluid-api.

![build status](https://io2gamelabs.visualstudio.com/_apis/public/build/definitions/d4e88719-08cf-42ab-bb30-bfc3b76a15ca/6/badge)

## Why

The main purpose was to recognize when a user draws a shape on a touch screen. The idea then grew more generic and it now recognizes shapes defined in a fluent way.

This is one example when you want to check if the shape begins with a down movement, a longer right movement and an up movement. The BoundsDescription sets a restriction on the entire shape.

```csharp
evaluator.Add("entry").When(p => 
	p.MovesDown()
	 .MovesRight()
	 .MovesUp()
	 .Bounds(BoundsDescriptor.IsWide));
```

The library also can define repeating parts. But it's still in alpha and there will be a lot of changes going forward.

## Sample app

The sample app is a Xamarin Forms app that allows you to draw different shapes and it resolves it on the fly.

![Sample app](http://imgur.com/a/RTTNH)

## Nuget

https://www.nuget.org/packages/patternizer

## Status

Still in alpha. The basic concept is covered but it needs work.

1. You prep a PatternEvaluator instance with one or more patterns to recognize
2. You pass a list of lines to the PatternEvaluator
3. It tells you if you have a match and what pattern it matches

I'm also refactoring a bunch of code to make it more like a framework so be prepared for stuff to change.

## Some definitions

The source data is always a list of lines. A line is a compound object of two points. I've choosen to define those within the library to avoid dependencies. I was thinking about exposing them through interfaces like  ILine and IPoint.

At the moment they are defines as structs.

```csharp
public struct Point
{
	public float X { get; set; }
	public float Y { get; set; }
}

public struct Line
{
	public Point P1 { get; set; }
	public Point P2 { get; set; }
}
```

I'm using Cartesian Coordinates which means that X is positive going right and Y is positive going up. This is kind of important to know.

## Example code

The most simple example could look like this.

```csharp

// create an evaluator
var evaluator = new PatternEvaluator();

// define what you are looking for (multiple patterns are ok)
evaluator.Add("button").When(Pattern.WideRectangle);
evaluator.Add("circle").When(Pattern.Circle);

// pass in a list of lines and get a result
var lines = MagicFunctionThatReturnsLines();
var result = evaluator.Evaluate(lines);
var isButton = result.IsValid() && result.Key == "button";

```

You can also chain and add bounds restrictions on the entire pattern or part of it. The bounds apply to all steps a head of the statement.

```csharp
evaluator.Add("entry").When(p => 
	p.MovesDown()
	 .MovesRight()
	 .MovesUp()
	 .Bounds(BoundsDescriptor.IsWide));
```

Or create patterns in functions and use those. There are also some predefined patterns exposed through the Pattern class. You can also define if the shape should End or Start near some relative position, like in the sample below it has to Start near the Top and Right side and end near the Bottom and Left side relative to the entire shape.

```csharp

evaluator.Add("button").When(Pattern.WideRectangle);
evaluator.Add("image").When(CreateCrossPattern());

public PatternBase CreateCrossPattern()
{
  var pattern = new StepPattern();

  pattern.MovesRightAndDown();
  pattern.MovesLeftAndDown()
         .Start(RelativePosition.NearTop | RelativePosition.NearRightSide)
         .End(RelativePosition.NearBottom | RelativePosition.NearLeftSide);

  return pattern;
}

public PatternBase CreateWideRectanglePattern()
{
	var pattern = new StepPattern ();
	pattern.MovesRight()
         .MovesDown()
         .MovesLeft()
         .MovesUp().End(RelativePosition.NearTop | RelativePosition.NearLeftSide)
         .Bounds(BoundsDescriptor.IsWide);
  	return pattern;
}
```

You can also define a pattern that allows inverse of all steps defined. In this pattern it doesn't matter if the drawing begins from left or right. It's essentially the same thing as flipping the input lines depending on which orientation you choose.

```csharp
public PatternBase CreateDoubleLinePattern()
{
    var pattern = new StepPattern();
    pattern.When(p => p.MovesRight().MovesLeft())
        .AllowInverse(InverseDescriptor.Horizontal);
    return pattern;
}

```
