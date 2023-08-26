using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using NUnit.Framework;

namespace Dotnet.MiniJira.Tests
{
    [TestFixture]
    public class TasksTests : MockedBaseTest
    {
        public Board boardToUse { get; set; }
        [SetUp]
        public void SetUp()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board",
                Description = "This is my test board"
            };

            boardToUse = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.IsNotNull(boardToUse, "Result should not be null");
            Assert.AreEqual(boardToCreate.Name, boardToUse.Name, "Name should not be null");
            Assert.AreEqual(boardToCreate.Description, boardToUse.Description, "Description should not be null"); ;
        }

        [TearDown]
        public void TearDown()
        {
            var result = _boardService.DeleteBoard(_adminUser, boardToUse.Id).Result;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsTrue(result);
        }

        [Test]
        public void Create_New_Task_Ok()
        {
        }

        [Test]
        public void Update_Existing_Task_Ok()
        {
        }

        [Test]
        public void Delete_Existing_Task_Ok()
        {
        }

        [Test]
        public void Delete_Not_Existing_Task_Exception()
        {
        }

        [Test]
        public void Move_Task_To_Another_Column_Ok()
        {
        }

        [Test]
        public void Move_Task_To_Invalid_Column_Exception()
        {
        }

        [Test]
        public void Tasks_Get_All_Sort_Field_Asc_Or_Default_Ok()
        {
        }

        [Test]
        public void Tasks_Get_All_Sort_Field_Desc_Ok()
        {
        }

        [Test]
        public void Tasks_Get_By_Id_Sort_Field_Asc_Or_Default_Ok()
        {
        }

        [Test]
        public void Tasks_Get_By_Id_Sort_Field_Desc_Ok()
        {
        }

        [Test]
        public void Task_Assigne_User_Ok()
        {
        }

        [Test]
        public void Task_Assigne_Invalid_User_Exception()
        {

        }

        [Test]
        public void Task_Add_Attachment_Ok()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board - delete column",
                Description = "This is my test board - delete column"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;
            Assert.IsNotNull(result, "Result should not be null");

            var createColumnModel = new CreateBoardColumnRequest
            {
                BoardId = result.Id,
                Column = new BoardColumns
                {
                    Description = "New custom column test",
                    Name = "New custom column test",
                    Type = Domain.Enums.Board.ColumnType.Custom
                }
            };

            var resultCreateColumn = _boardService.CreateColum(createColumnModel).Result;
            Assert.IsNotNull(resultCreateColumn?.Id);

            var deleteColumnModel = new DeleteBoardColumnRequest { BoardId = result.Id, ColumnId = createColumnModel.Column.Id };
            var resultDeleteColumn = _boardService.DeleteColum(deleteColumnModel).Result;
            Assert.IsNotNull(resultCreateColumn?.Id);
            Assert.IsNull(resultDeleteColumn.Columns.FirstOrDefault(p => p.Id == createColumnModel.Column.Id)?.Id);
        }

        [Test]
        public void Task_Add_Attachment_Exception()
        {

        }
    }
}