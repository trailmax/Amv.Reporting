namespace AmvReporting.Infrastructure.ModelEnrichers
{
    public interface IModelEnricher<TModel>
    {
        TModel Enrich(TModel model);
    }
}