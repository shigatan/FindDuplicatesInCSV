using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DuplicatesInCSV;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindDuplicatesByColumnInCsv
{

    /// <summary>
    /// Tests:
    //  pass valid file 
    //  pass empty file
    //  pass non-existed file
    //  pass file without column
    //  pass large files
    //  pass file with unique value in column
    //  pass file with same value in column
    /// </summary>
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestInvalidFile()
        {
            string filePath = "non existed file";
            var duplicatesFinder = new DuplicatesFinderFacade();
            var actual = duplicatesFinder.Run(filePath, "column1");
            Assert.IsFalse(actual.IsSuccess);
        }

        [TestMethod]
        public void TestInvalidColumn()
        {
            string filePath = "TestNonLarge1.csv";
            FileUtils.CreateFileWithFakeData(filePath, 10);
            var duplicatesFinder = new DuplicatesFinderFacade();
            var actual = duplicatesFinder.Run(filePath, "");
            Assert.IsFalse(actual.IsSuccess);
        }

        [TestMethod]
        public void TestNonLargeFileColumn1()
        {
            string NonLargeFile = "TestNonLarge1.csv";
            int keyColumn = 0;
            FileUtils.CreateFileWithFakeData(NonLargeFile, 500);
            var memoryWriter = new MemoryOutputWriter();
            var duplicatesFinder = new DuplicatesFinderInFile(NonLargeFile, keyColumn, memoryWriter);
            duplicatesFinder.FindDuplicates();

            var fileRows = File.ReadAllLines(NonLargeFile);
            var expected = fileRows.Select(x => x.Split(','))
                                .GroupBy(x => x[0]).ToDictionary(o => o.Key, o => o.ToList())
                                .Where(o => o.Value.Count > 1)
                                .SelectMany(x => x.Value)
                                .Select(x => string.Join(",", x));


            var actual = memoryWriter.Output.Select(x => x.Split(','))
                            .GroupBy(x => x[0]).ToDictionary(o => o.Key, o => o.ToList())
                            .Where(o => o.Value.Count > 1)
                            .SelectMany(x => x.Value)
                            .Select(x => string.Join(",", x));



            Assert.AreEqual(expected.Count(), actual.Count());           
        }

        [TestMethod]
        public void TestNonLargeFileColumnWithUniqueValues()
        {
            string NonLargeFile = "TestNonLarge1.csv";
            int keyColumn = 0;
            FileUtils.CreateFileWithFakeData(NonLargeFile, 1);
            var memoryWriter = new MemoryOutputWriter();
            var duplicatesFinder = new DuplicatesFinderInFile(NonLargeFile, keyColumn, memoryWriter);
            duplicatesFinder.FindDuplicates();

            var fileRows = File.ReadAllLines(NonLargeFile);
            var expected = fileRows.Select(x => x.Split(','))
                                .GroupBy(x => x[0]).ToDictionary(o => o.Key, o => o.ToList())
                                .Where(o => o.Value.Count > 1)
                                .SelectMany(x => x.Value)
                                .Select(x => string.Join(",", x));


            var actual = memoryWriter.Output.Select(x => x.Split(','))
                            .GroupBy(x => x[0]).ToDictionary(o => o.Key, o => o.ToList())
                            .Where(o => o.Value.Count > 1)
                            .SelectMany(x => x.Value)
                            .Select(x => string.Join(",", x));


            Assert.AreEqual(expected.Count(), actual.Count());           
        }



        [TestMethod]
        public void TestNonLargeFileColumn2()
        {
            string NonLargeFile = "TestNonLarge1.csv";
            FileUtils.CreateFileWithFakeData(NonLargeFile, 500);
            var duplicatesFinder = new DuplicatesFinderInFile(NonLargeFile, 1, new MemoryOutputWriter());
            duplicatesFinder.FindDuplicates();
        }


        [TestMethod]
        public void TestLargeFileColumn1()
        {
            try
            {
                string largeFile = "TestLarge.csv";
                FileUtils.CreateFileWithFakeData(largeFile);
                var duplicatesFinder = new DuplicatesFinderInLargeFile(largeFile, 0, new MemoryOutputWriter());
                duplicatesFinder.FindDuplicates();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
