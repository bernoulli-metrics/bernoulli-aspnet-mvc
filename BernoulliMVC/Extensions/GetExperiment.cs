using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Bernoulli;
using System.Web.Configuration;

namespace BernoulliMVC.Extensions
{
    public static class HtmlHelpersExtensions
    {
        public static Variant GetExperiment(this HtmlHelper helper, string experimentId, string userId)
        {
            return new Variant(helper, experimentId, userId);
        }
    }

    public class Variant : IDisposable
    {
        private readonly HtmlHelper _helper;
        private Experiment experiment;
        public Variant(HtmlHelper helper, string experimentId, string userId)
        {
            _helper = helper;
            string clientId = WebConfigurationManager.AppSettings["BernoulliClientID"];
            List<Bernoulli.Experiment> experiments = Bernoulli.Client.GetExperiments(clientId, new List<string> { experimentId }, userId, null);

            if (experiments != null && experiments.Count == 1)
            {
                experiment = experiments.First();
            }
        }

        public bool IsVariant(string variantId, bool control = false)
        {
            if (experiment == null) {
                return control;
            }

            return variantId == experiment.Variant;
        }

        public void Dispose()
        {
            experiment = null;
        }
    }
}