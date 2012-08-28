namespace AspUnitRunner.Core {
    internal class ResultDetail : IResultDetail {
        private ResultType _type;
        private string _name;
        private string _description;

        internal ResultDetail(ResultType type, string name, string description) {
            _type = type;
            _name = name;
            _description = description;
        }

        public ResultType Type {
            get { return _type; }
        }

        public string Name {
            get { return _name; }
        }

        public string Description {
            get { return _description; }
        }
    }
}
