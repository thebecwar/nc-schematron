using System;
using System.Collections.Generic;
using System.Text;

namespace NCSchematron
{
    public class ValidationResult
    {
        public ValidationResult(List<Types.PatternResult> results)
        {
            this.PatternResults = results;
        }
        public ValidationResult(Dictionary<string, Types.PhaseResult> results)
        {
            this.PhasedResults = results;
        }
        public List<Types.PatternResult> PatternResults { get; set; }
        public Dictionary<string, Types.PhaseResult> PhasedResults { get; set; }
        public bool IsPhasedResult => PhasedResults != null;

        private void WritePattern(StringBuilder sb, Types.PatternResult pattern, int indent, int indentStep = 4)
        {
            string pad = "".PadLeft(indent);
            sb.Append($"{pad}Pattern: {(pattern.Pattern.Id ?? "Anonymous")}");
            sb.AppendLine($" ({(pattern.PatternFired ? "" : "Not ")}Fired)");
            foreach (var rule in pattern.RuleResults)
            {
                WriteRule(sb, rule, indent + indentStep, indentStep);
            }
        }
        private void WritePatternErrors(StringBuilder sb, Types.PatternResult pattern, int indent, int indentStep = 4)
        {
            string pad = "".PadLeft(indent);
            sb.Append($"{pad}Pattern: {(pattern.Pattern.Id ?? "Anonymous")}");
            sb.AppendLine($" ({(pattern.PatternFired ? "" : "Not ")}Fired)");
            foreach (var rule in pattern.RuleResults)
            {
                WriteRule(sb, rule, indent + indentStep, indentStep);
            }
        }
        private void WriteRule(StringBuilder sb, Types.RuleResult rule, int indent, int indentStep = 4)
        {
            string pad = "".PadLeft(indent);
            sb.Append($"{pad}Rule: {(rule.Rule.Id ?? "Anonymous")}");
            sb.AppendLine($" ({(rule.RuleFired ? "" : "Not ")}Fired)");
            foreach (var assert in rule.ExecutedAssertions)
            {
                WriteAssertion(sb, assert, indent + indentStep, indentStep);
            }
        }
        private void WriteAssertion(StringBuilder sb, Types.EvaluationResult assertion, int indent, int indentStep = 4)
        {
            string pad = "".PadLeft(indent);
            string widePad = "".PadLeft(indent + indentStep);
            string dblWide = "".PadLeft(indent + 2 * indentStep);
            sb.Append($"{pad}Assertion: {(assertion.Assertion.Id ?? "Anonymous")}");
            sb.AppendLine($" ({(assertion.IsError ? "Failed" : "Passed")} - {assertion.Assertion.Test}");
            sb.AppendLine($"{widePad}Context:");
            sb.AppendLine($"{dblWide}Element: {assertion.ContextElement}");
            sb.AppendLine($"{dblWide}Line: {assertion.ContextLine} Position: {assertion.ContextPosition}");
            sb.AppendLine($"{widePad}Message:");
            if (String.IsNullOrWhiteSpace(assertion.Message))
            {
                sb.AppendLine($"{dblWide}{{Empty Message}}");
            }
            else
            {
                var lines = assertion.Message.Trim().Split('\n');
                foreach (var line in lines)
                {
                    sb.AppendLine($"{dblWide}{line.Trim()}");
                }
            }
        }
        public string GetResultString(int indentStep = 4)
        {
            StringBuilder sb = new StringBuilder();
            if (this.IsPhasedResult)
            {
                foreach (var key in this.PhasedResults.Keys)
                {
                    sb.AppendLine($"Phase: {key}");
                    var phaseResult = this.PhasedResults[key];
                    foreach (var pattern in phaseResult.PatternResults)
                    {
                        WritePattern(sb, pattern, indentStep, indentStep);
                    }
                }
            }
            else
            {
                foreach (var pattern in this.PatternResults)
                {
                    WritePattern(sb, pattern, 0, indentStep);
                }
            }
            return sb.ToString();
        }
        public string GetResultStringErrorsOnly(int indentStep = 4)
        {
            StringBuilder sb = new StringBuilder();
            if (this.IsPhasedResult)
            {
                foreach (var key in this.PhasedResults.Keys)
                {
                    //sb.AppendLine($"Phase: {key}");
                    var phaseResult = this.PhasedResults[key];
                    foreach (var pattern in phaseResult.PatternResults)
                    {
                        foreach (var rule in pattern.RuleResults)
                        {
                            bool hasErrorOnAssertion = false;
                            int index = sb.Length;
                            foreach (var assert in rule.ExecutedAssertions)
                            {                                
                                if (assert.IsError)
                                {
                                    hasErrorOnAssertion = true;
                                    WriteAssertion(sb, assert, indentStep, indentStep);
                                }                                
                            }
                            if (hasErrorOnAssertion)
                            {
                                sb.Insert(index, $"Rule: {(rule.Rule.Id ?? "-")}");
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var pattern in this.PatternResults)
                {
                    foreach (var rule in pattern.RuleResults)
                    {
                        bool hasErrorOnAssertion = false;
                        int index = sb.Length;
                        foreach (var assert in rule.ExecutedAssertions)
                        {
                            if (assert.IsError)
                            {
                                hasErrorOnAssertion = true;
                                WriteAssertion(sb, assert, indentStep, indentStep);
                            }
                        }
                        if (hasErrorOnAssertion)
                        {
                            sb.Insert(index, $"Rule: {(rule.Rule.Id ?? "-")}");
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}
