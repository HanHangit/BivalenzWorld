﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validator
{
    public class PL1Structure
    {
        private PL1DataStructure _pL1DataStructure;
        private List<string> _universum = new List<string>();
        private string _universeIdentifier = "u";


        public PL1Structure()
        {
            _pL1DataStructure = new PL1DataStructure(new ConstDictionary(), new PredicateDictionary(), new FunctionDictionary());
        }


        private bool CheckPredicateList(List<string> identifier, List<string> checkList)
        {
            if (identifier.Count != checkList.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < identifier.Count; i++)
                {
                    if (identifier[i] != checkList[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<string> CreateUniverseConsts(params string[] arguments)
        {
            List<string> universeIdentifier = new List<string>();

            foreach (var argument in arguments)
            {
                if (_pL1DataStructure.Consts.ContainsKey(argument))
                {
                    universeIdentifier.Add(_pL1DataStructure.Consts[argument]);
                }
                else
                {
                    throw new Exception("Argument " + argument + " is not in Dictionary");
                }
            }

            return universeIdentifier;
        }

        private void AddFunctionKeyToDictionary(ListDictionary dictionary, string function, List<string> arguments, string resultArgument)
        {
            if (!dictionary.ContainsKey(arguments))
            {
                dictionary[arguments] = resultArgument;
            }
        }


        public void AddConst(string key)
        {
            ConstDictionary consts = _pL1DataStructure.Consts;

            if (!consts.ContainsKey(key))
            {
                consts.Add(key, _universeIdentifier + consts.Count);
            }
        }


        public void AddFunctions(string function, List<string> arguments, string resultArgument)
        {
            FunctionDictionary functions = _pL1DataStructure.Functions;
            List<string> unviverseIdentifier = CreateUniverseConsts(arguments.ToArray());
            List<string> unviverseIdentifierResult = CreateUniverseConsts(resultArgument);

            if (functions.ContainsKey(function))
            {
                AddFunctionKeyToDictionary(functions[function], function, unviverseIdentifier, unviverseIdentifierResult.First());
            }
            else
            {
                functions.Add(function, new ListDictionary());
                AddFunctionKeyToDictionary(functions[function], function, unviverseIdentifier, unviverseIdentifierResult.First());
            }
        }

        public void AddPredicate(string predicate, List<string> arguments)
        {
            PredicateDictionary predicates = _pL1DataStructure.Predicates;

            List<string> universeIdentifier = CreateUniverseConsts(arguments.ToArray());

            if (predicates.ContainsKey(predicate))
            {
                //--Refactorn--//
                if (!(predicates[predicate].Any(p => CheckPredicateList(universeIdentifier, p))))
                {
                    predicates[predicate].Add(universeIdentifier);
                }
            }
            else
            {
                predicates.Add(predicate, new List<List<string>>() { universeIdentifier });
            }
        }

        //--Refactorn--//
        public ConstDictionary GetConsts() => _pL1DataStructure.Consts;
        public PredicateDictionary GetPredicates() => _pL1DataStructure.Predicates;
        public FunctionDictionary GetFunctions() => _pL1DataStructure.Functions;

    }


    public class ConstDictionary : Dictionary<string, string>
    {

    }

    public class PredicateDictionary : Dictionary<string, List<List<string>>>
    {

    }

    public class FunctionDictionary : Dictionary<string, ListDictionary>
    {

    }

    public class ListDictionary : Dictionary<List<string>, string>
    {
        public ListDictionary() : base(new ListComparer())
        {

        }

        private class ListComparer : IEqualityComparer<List<string>>
        {
            public bool Equals(List<string> x, List<string> y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(List<string> obj)
            {
                int hashcode = 0;
                foreach (string t in obj)
                {
                    hashcode ^= t.GetHashCode();
                }
                return hashcode;
            }
        }
    }
}
