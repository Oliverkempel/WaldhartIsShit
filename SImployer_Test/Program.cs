// See https://aka.ms/new-console-template for more information
using SImployer_Test;

Console.WriteLine("Hello, World!");

CapitechWebScraper ws = new CapitechWebScraper();

ws.Url = "https://hafjell.capitech.no/Apps/MinCapitech/";

ws.getTokens();
