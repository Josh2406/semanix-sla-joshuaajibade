using System;
using System.Collections.Generic;
using System.Linq;
namespace FormsService.Infrastructure.Metrics
{
    public interface IFormsMetrics
    {
        void IncrementFormPublished();
    }
}
