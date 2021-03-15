using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Autoveille
{

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.*",
                "~/Scripts/jquery-ui-{version}.js")
            );

            //bundles.Add(new ScriptBundle("~/Scripts/knockout").Include(
            //     "~/Scripts/Lib/knockout/knockout-{version}.js",
            //     "~/Scripts/Lib/knockout/knockout-deferred-updates.js")
            //);
        }
    }
}