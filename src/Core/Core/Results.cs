using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner.Core {
    internal class Results : IResults {
        internal Results() {
        }

        public int Tests { get; internal set; }
        public int Errors { get; internal set; }
        public int Failures { get; internal set; }

        public bool Successful {
            get {
                return Errors == 0 && Failures == 0;
            }
        }

        public IEnumerable<IResultDetail> DetailList { get; internal set; }
        public string Html { get; internal set; }

        public string Format() {
            return string.Format("{0}{2}{2}{1}{2}",
                FormatDetails(), FormatSummary(), Environment.NewLine);
        }

        internal string FormatDetails() {
            var newline = "";
            var stringBuilder = new StringBuilder();
            foreach (var detail in DetailList) {
                stringBuilder.Append(newline);
                stringBuilder.AppendFormat("{0}: {1}: {2}",
                    detail.Type, detail.Name, detail.Description);
                newline = Environment.NewLine;
            }

            return stringBuilder.ToString();
        }

        internal string FormatSummary() {
            return string.Format("Tests: {0}, Errors: {1}, Failures: {2}",
                Tests, Errors, Failures);
        }
    }
}
