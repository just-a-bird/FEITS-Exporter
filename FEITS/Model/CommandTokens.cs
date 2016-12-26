using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace FEITS.Model
{
    public static class CommandTokens
    {
        #region Constants

        //TODO(Robin): Give these descriptive names
        public const string Prefix = "$",
                            ParameterDelim = "|",
                            Newline = "\n";

        public const string Wa = "$Wa",
                            Wc = "$Wc",
                            a = "$a",
                            Nu = "$Nu",
                            N0 = "$N0",
                            N1 = "$N1",
                            kn = "$k\\n",
                            kp = k + p,
                            k = "$k",
                            t0 = "$t0",
                            t1 = "$t1",
                            p = "$p",
                            Wd = "$Wd",
                            Wv = "$Wv",
                            E = "$E",
                            Sbs = "$Sbs",
                            Svp = "$Svp",
                            Sre = "$Sre",
                            Fw = "$Fw",
                            Ws = "$Ws",
                            VF = "$VF",
                            Ssp = "$Ssp",
                            Fo = "$Fo",
                            VNMPID = "$VNMPID",
                            Fi = "$Fi",
                            b = "$b",
                            w = "$w",
                            l = "$l",
                            Wm = "$Wm",
                            Sbv = "$Sbv",
                            Sbp = "$Sbp",
                            Sls = "$Sls",
                            Slp = "$Slp",
                            Srp = "$Srp";

        #endregion

        public static bool IsValid(string token)
        {
            return AllTokensSet.Contains(token);
        }

        public static bool HasPrefix(string token)
        {
            return token.StartsWith(Prefix);
        }

        public static string WithPrefix(string token)
        {
            return HasPrefix(token) ? token : Prefix + token;
        }

        public static byte ParamsCount(string token)
        {
            if (NoParamTokensSet.Contains(token))
                return 0;
            if (SingleParamTokensSet.Contains(token))
                return 1;
            if (DoubleParamTokensSet.Contains(token))
                return 2;
            Debug.Assert(!AllTokensSet.Contains(token), $"{token} not found in {nameof(AllTokensSet)}");
            throw new ArgumentOutOfRangeException(nameof(token), token, "Invalid command token.");
        }

        public static string NormalizeLineEndings(string line, bool useEnvironmentNewline = false)
        {
            line = line.Replace("\\n", "\n")
                       .Replace("$k\n", "$k\\n");
            if (useEnvironmentNewline)
                line = line.Replace("\n", Environment.NewLine);
            return line;
        }

        public static IEnumerable<string> GetAllTokens() => AllTokensSet;
        public static IEnumerable<string> GetNoParamTokens() => NoParamTokensSet;
        public static IEnumerable<string> GetSingleParamTokens() => SingleParamTokensSet;
        public static IEnumerable<string> GetDoubleParamTokens() => DoubleParamTokensSet;

        private static readonly ISet<string> AllTokensSet,
                                             NoParamTokensSet = new SortedSet<string>
                                             {
                                                 Wa,
                                                 Wc,
                                                 a,
                                                 Nu,
                                                 N0,
                                                 N1,
                                                 kn,
                                                 k,
                                                 t0,
                                                 t1,
                                                 p,
                                                 Wd,
                                                 Wv
                                             },
                                             SingleParamTokensSet = new SortedSet<string>
                                             {
                                                 E,
                                                 Sbs,
                                                 Svp,
                                                 Sre,
                                                 Fw,
                                                 Ws,
                                                 VF,
                                                 Ssp,
                                                 Fo,
                                                 VNMPID,
                                                 Fi,
                                                 b,
                                                 w,
                                                 l
                                             },
                                             DoubleParamTokensSet = new SortedSet<string>
                                             {
                                                 Wm,
                                                 Sbv,
                                                 Sbp,
                                                 Sls,
                                                 Slp,
                                                 Srp
                                             };

        static CommandTokens()
        {
            AllTokensSet = new SortedSet<string>();
            foreach (var set in new[] {NoParamTokensSet, SingleParamTokensSet, DoubleParamTokensSet})
            {
                foreach (var token in set)
                {
                    var result = AllTokensSet.Add(token);

                    //NOTE(Robin): Fails if duplicate token is encountered.
                    Debug.Assert(result, $"{token} successfully added to {nameof(AllTokensSet)}");
                }
            }
        }

        //TODO(Robin): Change to set and add to AllTokens
        public static readonly List<string> LineDelimiters = new List<string> {kp, kn};

        public static readonly string LineSplitPattern =
            $"(?<={string.Join(ParameterDelim, LineDelimiters.Select(Regex.Escape))})";
        
        public static IEnumerable<string> SplitLines(string message)
        {
            return Regex.Split(message, LineSplitPattern);
        }
    }
}