using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day04;

namespace Tests
{
    [TestFixture]
    public class Day04Tests
    {
        private static readonly string[] ExampleData =
        {
            "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd",
            "byr:1937 iyr:2017 cid:147 hgt:183cm",
            "",
            "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884",
            "hcl:#cfa07d byr:1929",
            "",
            "hcl:#ae17e1 iyr:2013",
            "eyr:2024",
            "ecl:brn pid:760753108 byr:1931",
            "hgt:179cm",
            "",
            "hcl:#cfa07d eyr:2025 pid:166559648",
            "iyr:2011 ecl:brn hgt:59in",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(2);
        }

        [TestCase("byr", "2002")]
        [TestCase("pid", "000000001")]
        [TestCase("hgt", "60in")]
        [TestCase("hgt", "190cm")]
        [TestCase("hcl", "#123abc")]
        [TestCase("ecl", "brn")]
        [TestCase("eyr", "2025")]
        [TestCase("pid", "087499704")]
        [TestCase("hgt", "74in")]
        [TestCase("ecl", "grn")]
        [TestCase("iyr", "2012")]
        [TestCase("eyr", "2030")]
        [TestCase("byr", "1980")]
        [TestCase("hcl", "#623a2f")]
        [TestCase("eyr", "2029")]
        [TestCase("ecl", "blu")]
        [TestCase("cid", "129")]
        [TestCase("byr", "1989")]
        [TestCase("iyr", "2014")]
        [TestCase("pid", "896056539")]
        [TestCase("hcl", "#a97842")]
        [TestCase("hgt", "165cm")]
        [TestCase("hcl", "#888785")]
        [TestCase("hgt", "164cm")]
        [TestCase("byr", "2001")]
        [TestCase("iyr", "2015")]
        [TestCase("cid", "88")]
        [TestCase("pid", "545766238")]
        [TestCase("ecl", "hzl")]
        [TestCase("eyr", "2022")]
        [TestCase("iyr", "2010")]
        [TestCase("hgt", "158cm")]
        [TestCase("hcl", "#b6652a")]
        [TestCase("ecl", "blu")]
        [TestCase("byr", "1944")]
        [TestCase("eyr", "2021")]
        [TestCase("pid", "093154719")]
        public void ValidFieldTests(string name, string value)
        {
            Puzzle.IsFieldValid(name, value).Should().BeTrue();
        }

        [TestCase("byr", "2003")]
        [TestCase("pid", "0123456789")]
        [TestCase("hgt", "190in")]
        [TestCase("hgt", "190")]
        [TestCase("hgt", "190ab")]
        [TestCase("hcl", "#123abz")]
        [TestCase("hcl", "123abc")]
        [TestCase("ecl", "wat")]
        [TestCase("eyr", "1972")]
        [TestCase("eyr", "aaa")]
        [TestCase("eyr", "2031")]
        public void InvalidFieldTests(string name, string value)
        {
            Puzzle.IsFieldValid(name, value).Should().BeFalse();
        }
    }
}
