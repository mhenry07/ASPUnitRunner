﻿using System;
using System.Collections.Generic;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Core {
    internal class SelectorParser : ISelectorParser {
        private const string SelectorFormName = "frmSelector";
        private const string TestContainersSelectName = "cboTestContainers";
        private const string TestCasesSelectName = "cboTestCases";

        private readonly IHtmlDocumentFactory _htmlDocumentFactory;

        public SelectorParser(IHtmlDocumentFactory htmlDocumentFactory) {
            _htmlDocumentFactory = htmlDocumentFactory;
        }

        public IEnumerable<string> ParseContainers(string html) {
            var document = _htmlDocumentFactory.Create(html);
            var selectorForm = GetSelectorForm(document);
            var testContainersSelect = GetSelectElement(selectorForm, TestContainersSelectName);
            return ParseSelectorOptions(testContainersSelect);
        }

        public IEnumerable<string> ParseTestCases(string html, string container) {
            throw new NotImplementedException();
        }

        private static IHtmlElement GetSelectorForm(IHtmlDocument document) {
            var forms = document.GetElementsByTagName("FORM");
            return GetFirstElementByName(forms, SelectorFormName);
        }

        private static IHtmlElement GetSelectElement(IHtmlElement form, string name) {
            var selectElements = form.GetElementsByTagName("SELECT");
            return GetFirstElementByName(selectElements, name);
        }

        private static IHtmlElement GetFirstElementByName(IHtmlCollection elements, string name) {
            foreach (var element in elements)
                if (string.Equals(element.GetAttribute("NAME"), name, StringComparison.Ordinal))
                    return element;
            throw new FormatException("Unexpected response.");
        }

        // ignores the first option (All Test Containers or All Test Cases)
        private static IEnumerable<string> ParseSelectorOptions(IHtmlElement selectElement) {
            var result = new List<string>();
            var optionElements = HtmlElementParser.GetOptionElements(selectElement.InnerHtml);
            foreach (var option in optionElements)
                if (option != optionElements.First)
                    result.Add(option.Text);
            return result;
        }
    }
}
