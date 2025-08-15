namespace FormsService.Infrastructure.Metrics
{
    public class FormsMetrics : IFormsMetrics
    {
        private readonly Counter _formsPublishedCounter;

        public FormsMetrics()
        {
            _formsPublishedCounter = Prometheus.Metrics.CreateCounter(
               "fe_forms_published_total",
               "Total number of forms published."
);
        }

        public void IncrementFormPublished()
        {
            _formsPublishedCounter.Inc();
        }
    }
}
