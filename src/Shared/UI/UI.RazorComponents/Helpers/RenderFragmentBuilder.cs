namespace UI.Razor.Helpers;

public static class RenderFragmentBuilder
{
    public static RenderFragment Create<TFragment>(Dictionary<string, object>? parameters = null) where TFragment : ComponentBase => builder =>
    {
        builder.OpenComponent<TFragment>(0);
        if (parameters is not null)
        {
            foreach (var parameter in parameters)
            {
                builder.AddAttribute(1, parameter.Key, parameter.Value);
            }
        }
        builder.CloseComponent();
    };
}
