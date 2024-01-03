using System.ComponentModel.DataAnnotations;
using AdventOfCode.Types;

namespace AdventOfCode.Options;

public class PuzzleOptions
{
    [Required]
    public int Year { get; set; }
    [Required]
    public int Day { get; set; }
    [Required]
    public Part Part { get; set; }
}