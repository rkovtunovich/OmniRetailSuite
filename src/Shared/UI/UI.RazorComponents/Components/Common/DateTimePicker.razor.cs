namespace UI.Razor.Components.Common;

public partial class DateTimePicker
{
    [Parameter] public DateTimeOffset Value { get; set; }

    [Parameter] public EventCallback<DateTimeOffset> ValueChanged { get; set; }

    [Parameter] public DateTimeOffset? NullableValue { get; set; }

    [Parameter] public EventCallback<DateTimeOffset?> NullableValueChanged { get; set; }

    [Parameter] public string? Label { get; set; } = null;

    [Parameter] public string? LabelTime { get; set; } = null;

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public bool UseNullable { get; set; }

    [Parameter] public string RequiredError { get; set; } = "Required";

    private DateTime? _dateTime;

    protected override async Task OnParametersSetAsync()
    {
        if (UseNullable)
            _dateTime = NullableValue.HasValue ? NullableValue.Value.LocalDateTime : null;  
        else
            _dateTime = Value.LocalDateTime;

        await base.OnParametersSetAsync();
    }

    private string DatePattern => CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

    private string DatePatternLong => "ddd, " + CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern;

    private DayOfWeek FirstDayOfWeek => CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

    private bool TimeAmPm => CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.EndsWith("tt");

    async Task DateChanged(DateTime? dt)
    {
        if (dt.HasValue)
        {
            _dateTime = new DateTime(
                dt.Value.Year,
                dt.Value.Month,
                dt.Value.Day,
                _dateTime.HasValue ? _dateTime.Value.Hour : 0,
                _dateTime.HasValue ? _dateTime.Value.Minute : 0,
                0
            );

            await DateTimeChanged();
        }
        else
        {
            if (UseNullable)
            {
                _dateTime = null;
                await NullableValueChanged.InvokeAsync(null);
            }
        }
    }

    async Task TimeChanged(TimeSpan? ts)
    {
        if (ts.HasValue)
        {
            _dateTime = new DateTime(
                _dateTime.HasValue ? _dateTime.Value.Year : DateTime.Now.Year,
                _dateTime.HasValue ? _dateTime.Value.Month : DateTime.Now.Month,
                _dateTime.HasValue ? _dateTime.Value.Day : DateTime.Now.Day,
                ts.Value.Hours,
                ts.Value.Minutes,
                0
            );

            await DateTimeChanged();
        }
        else
        {
            if (UseNullable)
            {
                _dateTime = null;
                await NullableValueChanged.InvokeAsync(null);
            }
        }
    }

    async Task DateTimeChanged()
    {
        TimeZoneInfo tzi = TimeZoneInfo.Local;
        DateTimeOffset dateTimeOffset = new DateTimeOffset(
            _dateTime.HasValue ? _dateTime.Value.Year : DateTime.Now.Year,
            _dateTime.HasValue ? _dateTime.Value.Month : DateTime.Now.Month,
            _dateTime.HasValue ? _dateTime.Value.Day : DateTime.Now.Day,
            _dateTime.HasValue ? _dateTime.Value.Hour : DateTime.Now.Hour,
            _dateTime.HasValue ? _dateTime.Value.Minute : DateTime.Now.Minute,
            0,
            tzi.GetUtcOffset(_dateTime.HasValue ? _dateTime.Value : DateTime.Now)
            );

        DateTimeOffset? ndateTimeOffset = new DateTimeOffset?(dateTimeOffset);

        await ValueChanged.InvokeAsync(dateTimeOffset);
        await NullableValueChanged.InvokeAsync(ndateTimeOffset);
    }
}
