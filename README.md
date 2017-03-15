# Patternizer
A great little library for resolving patterns in a list of lines using a fluid-api.

##Status

Still in alpha. The basic concept is covered but it needs work.

1. You prep a PatternEvaluator instance with one or more patterns to recognize
2. You pass a list of lines to the PatternEvaluator
3. It tells you if you have a match and what pattern it matches

## Example code

The most simple example could look like this.

```csharp

// create an evaluator
var evaluator = new PatternEvaluator();

// define what you are looking for (multiple patterns are ok)
evaluator.Add("button").When(Pattern.WideRectangle);
evaluator.Add("cirle").When(Pattern.Circle);

// pass in a list of lines and get a result
var lines = new List<Point>() = MagicFunctionWithLines();
var result = evaluator.Evaluate(lines);
var isButton = result.IsValid();

```

You can also chain and add bounds restrictions on the entire pattern or part of it.

```csharp
evaluator.Add("entry").When(p => p.MovesDown().MovesRight().MovesUp().Bounds(BoundsDescriptor.IsWide));
```

Or create patterns in functions and use those

```csharp

evaluator.Add("button").When(Pattern.WideRectangle);
evaluator.Add("image").When(Pattern.Cross);

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
