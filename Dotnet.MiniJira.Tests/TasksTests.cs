namespace Dotnet.MiniJira.Tests;

using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using NUnit.Framework;

[TestFixture]
public class TasksTests : MockedBaseTest
{
    public Board boardToUse { get; set; }
    public CreateTaskRequest taskToCreate { get; set; }
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
        Assert.AreEqual(boardToCreate.Description, boardToUse.Description, "Description should not be null");

        taskToCreate = new CreateTaskRequest
        {
            BoardId = boardToUse.Id,
            ColumnId = boardToUse.Columns.FirstOrDefault(p => p.Name == "Todo")?.Id,
            Task = new CreateTaskItem
            {
                Name = "Test",
                Description = "Test",
                DeadLine = TimeZoneInfo.ConvertTimeToUtc(new DateTime(2028, 10, 10, 10, 10, 10, 10), TimeZoneInfo.Local),
                IsFavorite = true
            }
        };
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
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);
        Assert.IsNotNull(result?.Columns);
        Assert.IsNotNull(result?.Columns.Select(p => p.Tasks));
        Assert.Greater(result?.Columns.Select(p => p.Tasks)?.Count(), 0);

        var addedTask = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);

        var resultGet = _taskService.GetById(boardToUse.Id, addedTask.Id).Result;

        Assert.IsNotNull(resultGet?.Id);
        Assert.AreEqual(resultGet.Id, addedTask.Id);
        Assert.AreEqual(resultGet.Name, addedTask.Name);
        Assert.AreEqual(resultGet.Description, addedTask.Description);
        Assert.AreEqual(resultGet.IsFavorite, addedTask.IsFavorite);
        Assert.AreEqual(resultGet.DeadLine, addedTask.DeadLine);
    }

    [Test]
    public void Update_Existing_Task_Ok()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);
        Assert.IsNotNull(result?.Columns);
        Assert.IsNotNull(result?.Columns.Select(p => p.Tasks));
        Assert.Greater(result?.Columns.Select(p => p.Tasks)?.Count(), 0);

        var addedTask = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);

        var taskUpdateReq = new UpdateTaskRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTask.Id,
            Task = new UpdateTaskItem
            {
                DeadLine = TimeZoneInfo.ConvertTimeToUtc(new DateTime(2030, 10, 10, 10, 10, 10, 10), TimeZoneInfo.Local),
                Description = "Test Updated",
                IsFavorite = false,
                Name = "Test Updated"
            }
        };

        // Update the task values
        var resultUpdate = _taskService.UpdateTask(taskUpdateReq).Result;

        var resultGet = _taskService.GetById(boardToUse.Id, addedTask.Id).Result;

        Assert.IsNotNull(resultGet?.Id);
        Assert.AreEqual(resultGet.Id, addedTask.Id);
        Assert.AreEqual(resultGet.Name, taskUpdateReq.Task.Name);
        Assert.AreEqual(resultGet.Description, taskUpdateReq.Task.Description);
        Assert.AreEqual(resultGet.IsFavorite, taskUpdateReq.Task.IsFavorite);
        Assert.AreEqual(resultGet.DeadLine, taskUpdateReq.Task.DeadLine);
    }

    [Test]
    public void Delete_Existing_Task_Ok()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTask = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);

        // Update the task values
        var deletedTaskReq = new DeleteTaskRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTask.Id
        };

        var resultUpdate = _taskService.DeleteTask(deletedTaskReq).Result;
        Assert.IsNotNull(resultUpdate);

        // Throw not found exception, means that it does not exist anymore
        Assert.Throws<AggregateException>(() => _taskService.GetById(boardToUse.Id, addedTask.Id).Wait());
    }

    [Test]
    public void Delete_Not_Existing_Task_Exception()
    {
        var deletedTaskReq = new DeleteTaskRequest
        {
            BoardId = "FakeIdToDelete",
            TaskId = "FakeIdToDelete"
        };

        // Throw not found exception, means that it does not exist anymore
        Assert.Throws<AggregateException>(() => _taskService.DeleteTask(deletedTaskReq).Wait());
    }

    [Test]
    public void Move_Task_To_Another_Column_Ok()
    {
        // Move column from Todo to In Progress
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTaskTodo = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskTodo?.Id);

        var inProgressColumnId = boardToUse.Columns.First(p => p.Name == "In progress").Id;

        var moveTaskReq = new MoveTaskRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTaskTodo.Id,
            NewColumnId = inProgressColumnId,
        };

        var resultMovedTask = _taskService.MoveTask(moveTaskReq).Result;
        Assert.IsNotNull(resultMovedTask.Id);

        var taskTodo = resultMovedTask?.Columns.First(p => p.Name == "Todo").Tasks?.FirstOrDefault(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNull(taskTodo);

        var addedTaskInProgress = resultMovedTask?.Columns.First(p => p.Name == "In progress").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskInProgress?.Id);
        Assert.AreEqual(addedTaskInProgress.Id, addedTaskTodo.Id);
        Assert.AreEqual(addedTaskInProgress.Name, addedTaskTodo.Name);
        Assert.AreEqual(addedTaskInProgress.Description, addedTaskTodo.Description);
        Assert.AreEqual(addedTaskInProgress.IsFavorite, addedTaskTodo.IsFavorite);
        Assert.AreEqual(addedTaskInProgress.DeadLine, addedTaskTodo.DeadLine);
    }

    [Test]
    public void Move_Task_To_Invalid_Column_Exception()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTaskTodo = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskTodo?.Id);

        var inProgressColumnId = boardToUse.Columns.First(p => p.Name == "In progress").Id;

        var moveTaskReq = new MoveTaskRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTaskTodo.Id,
            NewColumnId = "InvalidColumnIdWillThrowExcpetion",
        };

        Assert.Throws<AggregateException>(() => _taskService.MoveTask(moveTaskReq).Wait());
    }

    [Test]
    public void Task_Assigne_User_Ok()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTaskTodo = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskTodo?.Id);

        var assignTaskReq = new AssignTaskRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTaskTodo.Id,
            UserId = _testUser.Id
        };

        var resultAssignTask = _taskService.Assign(assignTaskReq).Result;
        Assert.IsNotNull(resultAssignTask?.Id);

        var resultGet = _taskService.GetById(boardToUse.Id, addedTaskTodo.Id).Result;
        Assert.AreEqual(resultGet.Assignee, _testUser.Id);
    }

    [Test]
    public void Task_Assigne_Invalid_User_Exception()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTaskTodo = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskTodo?.Id);

        var assignTaskReq = new AssignTaskRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTaskTodo.Id,
            UserId = "InvalidUserIdWillThrowException"
        };

        Assert.Throws<AggregateException>(() => _taskService.Assign(assignTaskReq).Wait());
    }

    [Test]
    public void Task_Add_Attachment_Ok()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTaskTodo = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskTodo?.Id);

        var addAttachRed = new AddAttachmentRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTaskTodo.Id,
            Attachments = new List<string>
            {
                "https://myimageweb-sites.com/dog.png",
                "https://myimageweb-sites.com/cat.png",
                "https://myimageweb-sites.com/bear.png"
            }
        };

        var resultAddAttachTask = _taskService.AddAttachments(addAttachRed).Result;
        Assert.IsNotNull(resultAddAttachTask?.Id);

        var resultGet = _taskService.GetById(boardToUse.Id, addedTaskTodo.Id).Result;
        Assert.AreEqual(resultGet.Attachments.Count, 3);
        Assert.AreEqual(resultGet.Attachments[0], "https://myimageweb-sites.com/dog.png");
        Assert.AreEqual(resultGet.Attachments[1], "https://myimageweb-sites.com/cat.png");
        Assert.AreEqual(resultGet.Attachments[2], "https://myimageweb-sites.com/bear.png");
    }

    [Test]
    public void Task_Add_Attachment_Exception()
    {
        var result = _taskService.CreateTask(_devUser, taskToCreate).Result;
        Assert.IsNotNull(result?.Id);

        var addedTaskTodo = result?.Columns.First(p => p.Name == "Todo").Tasks?.First(p => p.Name == taskToCreate.Task.Name);
        Assert.IsNotNull(addedTaskTodo?.Id);

        var addAttachRed = new AddAttachmentRequest
        {
            BoardId = boardToUse.Id,
            TaskId = addedTaskTodo.Id,
            Attachments = new List<string>()
        };

        Assert.Throws<AggregateException>(() => _taskService.AddAttachments(addAttachRed).Wait());
    }
}