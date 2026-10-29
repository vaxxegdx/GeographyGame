namespace GeographyGame.Backend.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? FlagImgPath { get; set; }
    public string? OnMapImgPath { get; set; }
    public string? OutlineImgPath { get; set; }
}