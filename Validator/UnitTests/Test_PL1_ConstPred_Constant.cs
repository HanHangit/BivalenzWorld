﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validator;

namespace UnitTests
{
    [TestClass]
    public class Test_PL1_ConstPred_Constant
    {
        private ConstDictionary _constDictionary = new ConstDictionary();
        private FunctionDictionary _funcDictionary = new FunctionDictionary();
        private PredicateDictionary _predDictionary = new PredicateDictionary();


        private WorldParameter CreateTestParmeter()
        {
            List<WorldObject> worldObjects = new List<WorldObject>
            {
                new WorldObject(new List<string> { "a", "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.BIG }, null),
                new WorldObject(new List<string> { "b", "c" }, new List<string> {TarskiWorldDataFields.TET, TarskiWorldDataFields.SMALL }, null),
                new WorldObject(new List<string> { "b", "d" }, new List<string> {TarskiWorldDataFields.CUBE, TarskiWorldDataFields.BIG }, null),
                new WorldObject(new List<string> { "c" }, new List<string> {TarskiWorldDataFields.CUBE }, null)
            };

            return new WorldParameter(worldObjects, null);
        }


        [TestInitialize]
        public void PL1_Setup()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = CreateTestParmeter();
            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            _predDictionary = structure.GetPredicates();
            _constDictionary = structure.GetConsts();
            _funcDictionary = structure.GetFunctions();
        }

        [TestMethod]
        public void PL1_Constants_True()
        {
            TarskiWorld world = new TarskiWorld();
            WorldParameter parameter = CreateTestParmeter();

            world.Check(parameter);

            PL1Structure structure = world.GetPl1Structure();

            ConstDictionary consts = structure.GetConsts();
            PredicateDictionary preds = structure.GetPredicates();

            Assert.AreEqual(consts.Count, 4);

            Assert.AreEqual(consts["a"], "u0");
            Assert.AreEqual(consts["c"], "u1");
            Assert.AreEqual(consts["b"], "u2");
            Assert.AreEqual(consts["d"], "u3");
        }

        [TestMethod]
        public void PL1_Predicates_ContainsKey_True()
        {
            Assert.IsTrue(_predDictionary.ContainsKey(TarskiWorldDataFields.TET));
            Assert.IsTrue(_predDictionary.ContainsKey(TarskiWorldDataFields.BIG));
            Assert.IsTrue(_predDictionary.ContainsKey(TarskiWorldDataFields.SMALL));
            Assert.IsTrue(_predDictionary.ContainsKey(TarskiWorldDataFields.CUBE));
        }

        [TestMethod]
        public void PL1_Predicates_ContainsValue_True()
        {
            List<List<string>> consts = _predDictionary[TarskiWorldDataFields.TET];
            Assert.AreEqual(consts.Count, 3);

            Assert.IsTrue(consts.Any(c => c.Contains("u0")));
            Assert.IsTrue(consts.Any(c => c.Contains("u2")));
            Assert.IsTrue(consts.Any(c => c.Contains("u1")));
        }
    }
}
