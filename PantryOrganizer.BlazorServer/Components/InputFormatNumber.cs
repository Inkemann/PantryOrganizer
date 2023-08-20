using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PantryOrganizer.BlazorServer.Components;

public class InputFormatNumber<TValue>
    : InputBase<TValue>
{
    private static readonly string _stepAttributeValue = GetStepAttributeValue();

    private static string GetStepAttributeValue()
    {
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        return targetType == typeof(int) ||
            targetType == typeof(long) ||
            targetType == typeof(short) ||
            targetType == typeof(float) ||
            targetType == typeof(double) ||
            targetType == typeof(decimal)
            ? "any"
            : throw new InvalidOperationException(
                $"The type '{targetType}' is not a supported numeric type.");
    }

    [Parameter]
    public string? Format { get; set; }
    [Parameter]
    public string ParsingErrorMessage { get; set; }
        = "The {0} field must be a number.";
    [DisallowNull]
    public ElementReference? Element { get; protected set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "input");
        builder.AddAttribute(1, "step", _stepAttributeValue);
        builder.AddMultipleAttributes(2, AdditionalAttributes);
        builder.AddAttribute(3, "type", "number");
        if (CssClass != null)
            builder.AddAttribute(4, "class", CssClass);
        builder.AddAttribute(5, "value", BindConverter.FormatValue(CurrentValueAsString));
        builder.AddAttribute(
            6,
            "onchange",
            EventCallback.Factory.CreateBinder<string?>(
                this,
                __value => CurrentValueAsString = __value,
                CurrentValueAsString));
        builder.AddElementReferenceCapture(7, __inputReference => Element = __inputReference);
        builder.CloseElement();
    }

    protected override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out TValue result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(
                CultureInfo.InvariantCulture,
                ParsingErrorMessage,
                DisplayName ?? FieldIdentifier.FieldName);
            return false;
        }
    }

    protected override string? FormatValueAsString(TValue? value)
        => value switch
        {
            null => null,
            int @int => @int.ToString(Format, CultureInfo.InvariantCulture.NumberFormat),
            long @long => @long.ToString(Format, CultureInfo.InvariantCulture.NumberFormat),
            short @short => @short.ToString(Format, CultureInfo.InvariantCulture.NumberFormat),
            float @float => @float.ToString(Format, CultureInfo.InvariantCulture.NumberFormat),
            double @double => @double.ToString(Format, CultureInfo.InvariantCulture.NumberFormat),
            decimal @decimal => @decimal.ToString(Format, CultureInfo.InvariantCulture.NumberFormat),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
}
