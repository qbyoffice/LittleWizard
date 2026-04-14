using System;
using System.Text;
using LittleWizard.Api.Cards;

namespace LittleWizard.Api;

public static class CardPathUtils
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                    sb.Append('_');
                sb.Append(char.ToLower(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string GetCardImagePath(this Type cardType)
    {
        if (cardType == null)
            throw new ArgumentNullException(nameof(cardType));
        string snakeName = cardType.Name.ToSnakeCase();
        return $"res://{MainFile.ModId}/images/card_portraits/{snakeName}.png";
    }

    public static string GetCardImagePath(this LittleWizardCard card)
    {
        if (card == null)
            throw new ArgumentNullException(nameof(card));
        return card.GetType().GetCardImagePath();
    }
}
