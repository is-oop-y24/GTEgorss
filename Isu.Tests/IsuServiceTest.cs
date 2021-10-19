using System;
using Isu.Entities;
using Isu.MyClasses;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    [TestFixture]
    public class Tests
    {
        private IsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService(4);
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group newGroup = _isuService.AddGroup(new IsuGroupName("M3201"));
            _isuService.AddStudent(new Group(new IsuGroupName("M3201")), "a");
            
            Assert.AreEqual(_isuService.FindGroup(new IsuGroupName("M3201")).Students[0].Name, "a");
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group newGroup = _isuService.AddGroup(new IsuGroupName("M3201"));

                char name = 'a';
                for (int i = 0; i < 20; ++i)
                {
                    _isuService.AddStudent(newGroup, Char.ToString(name));
                    ++name;
                }
                
                _isuService.AddStudent(_isuService.FindGroup(new IsuGroupName("M3201")), "grazhdanin X");
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup(new IsuGroupName("M3501"));
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group1 = _isuService.AddGroup(new IsuGroupName("M3201"));
            _isuService.AddStudent(group1, "a");
            Group group2 = _isuService.AddGroup(new IsuGroupName("M3202"));
            _isuService.ChangeStudentGroup(_isuService.FindStudent("a"), group2);
            
            Assert.AreEqual(_isuService.FindGroup(new IsuGroupName("M3201")).Students.Count, 0);
            Assert.AreEqual(_isuService.FindGroup(new IsuGroupName("M3202")).Students[0].Name, "a");
        }
    }
}