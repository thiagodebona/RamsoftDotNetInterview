using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using NUnit.Framework;

namespace Dotnet.MiniJira.Tests
{
    [TestFixture]
    public class BoardTests : MockedBaseTest
    {
        [Test]
        public void Create_New_Board_Ok()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board - Tasks test",
                Description = "This is my test board"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(boardToCreate.Name, result.Name, "Name should not be null");
            Assert.AreEqual(boardToCreate.Description, result.Description, "Description should not be null"); ;
        }

        [Test]
        public void Create_New_Board_Check_GetById_Ok()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board",
                Description = "This is my test board"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(boardToCreate.Name, result.Name, "Name should not be null");
            Assert.AreEqual(boardToCreate.Description, result.Description, "Description should not be null");


            var resultGetById = _boardService.GetById(result.Id).Result;
            Assert.IsNotNull(result?.Id, "Result should not be null");
        }

        [Test]
        public void Create_New_Board_Duplicated_Exception()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board2",
                Description = "This is my test board"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.IsNotNull(result, "Result should not be null");

            Assert.Throws<AggregateException>(() => _boardService.CreateBoard(_adminUser, boardToCreate).Wait());
        }

        [Test]
        public void Create_New_Board_Not_Admin_Exception()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board3",
                Description = "This is my test board"
            };

            Assert.Throws<AggregateException>(() => _boardService.CreateBoard(_devUser, boardToCreate).Wait());
        }

        [Test]
        public void Delete_Board_Should_Ok()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board test xxx",
                Description = "This is my test board"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.NotNull(result?.Id);

            var resultDelete = _boardService.DeleteBoard(_adminUser, result.Id).Result;
            Assert.IsTrue(resultDelete);

            Assert.Throws<AggregateException>(() => _boardService.CreateBoard(_devUser, boardToCreate).Wait());
        }

        [Test]
        public void Delete_Inexistent_Board_Exception()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board 2 asdddd",
                Description = "This is my test board 2"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.NotNull(result?.Id);

            var resultDelete = _boardService.DeleteBoard(_adminUser, result.Id).Result;
            Assert.IsTrue(resultDelete);

            Assert.Throws<AggregateException>(() => _boardService.DeleteBoard(_adminUser, result.Id).Wait());
        }

        [Test]
        public void Delete_Board_Not_Admin_Exception()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board 2",
                Description = "This is my test board 2"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.NotNull(result?.Id);

            Assert.Throws<AggregateException>(() => _boardService.DeleteBoard(_devUser, result.Id).Wait());
        }

        [Test]
        public void Update_Board_Ok()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board - test",
                Description = "This is my test board - test"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(boardToCreate.Name, result.Name, "Name should not be null");
            Assert.AreEqual(boardToCreate.Description, result.Description, "Description should not be null"); ;

            var boardToUpdate = new UpdateBoardRequest
            {
                BoardId = result.Id,
                Name = "My new board - updated",
                Description = "This is my test board - updated"
            };

            var resultUpdate = _boardService.UpdateBoard(_adminUser, boardToUpdate).Result;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreNotEqual(resultUpdate.Name, result.Name, "Name should not be equal");
            Assert.AreNotEqual(resultUpdate.Description, result.Description, "Description should not be equal"); ;
        }

        [Test]
        public void Update_Board_Bad_User_Profile_Exception()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board - old",
                Description = "This is my test board - old"
            };

            var result = _boardService.CreateBoard(_adminUser, boardToCreate).Result;

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(boardToCreate.Name, result.Name, "Name should not be null");
            Assert.AreEqual(boardToCreate.Description, result.Description, "Description should not be null"); ;

            var boardToUpdate = new UpdateBoardRequest
            {
                BoardId = result.Id,
                Name = "My new board - updated",
                Description = "This is my test board - updated"
            };

            Assert.Throws<AggregateException>(() => _boardService.UpdateBoard(_devUser, boardToUpdate).Wait());
        }

        [Test]
        public void Board_Create_Column()
        {
            var boardToCreate = new CreateBoardRequest
            {
                Name = "My new board - create column",
                Description = "This is my test board - create column"
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

            var justAddedColumn = resultCreateColumn.Columns.FirstOrDefault(p=> p.Id == createColumnModel.Column.Id);
            Assert.IsNotNull(justAddedColumn?.Id);
            Assert.AreEqual(justAddedColumn.Name, createColumnModel.Column.Name);
            Assert.AreEqual(justAddedColumn.Description, createColumnModel.Column.Description);
            Assert.AreEqual(justAddedColumn.Type, createColumnModel.Column.Type);
        }

        [Test]
        public void Delete_Column_Without_Tasks()
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
    }
}