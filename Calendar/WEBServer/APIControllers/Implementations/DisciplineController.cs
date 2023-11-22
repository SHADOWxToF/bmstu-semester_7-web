using BL.ForAPI.DTO;
using BL.ForAPI.Interfaces;
using BL.Models.Interfaces;
using BL.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net;

namespace WEBServer.APIControllers.Implementations
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userID}")]
    public class DisciplineController : ControllerBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private IDiscipline discipline;
        public DisciplineController(IDiscipline discipline)
        {
            this.discipline = discipline;
        }

        [HttpGet("disciplines")]
        public async Task<ActionResult<List<DisciplineData>>> GetDisciplines(int userID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            List<DisciplineData>? disciplines = null;
            try
            {
                disciplines = await discipline.GetDisciplines(userID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (disciplines != null)
                return disciplines;
            else
                return new List<DisciplineData>();
        }

        [HttpGet("discplines/{disciplineID}")]
        public async Task<ActionResult<DisciplineData>> GetDiscipline(int userID, int disciplineID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            DisciplineData? disciplineData = null;
            try
            {
                disciplineData = await discipline.GetDiscipline(userID, disciplineID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (disciplineData != null)
                return disciplineData;
            else
                return StatusCode(404);
        }

        [HttpPost("disciplines")]
        public async Task<ActionResult> CreateDiscipline(int userID, DisciplineData disciplineData)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID || userID != disciplineData.UID)
                return StatusCode(403);
            try
            {
                await discipline.CreateDiscipline(disciplineData);
                return StatusCode(201);
            }
            catch (ExistingName)
            {
                return StatusCode(409); // Conflict
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }

        [HttpPut("disciplines/{disciplineID}")]
        public async Task<ActionResult> UpdateDiscipline(int userID, int disciplineID, DisciplineData disciplineData)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID || userID != disciplineData.UID)
                return StatusCode(403);
            try
            {
                await discipline.UpdateDiscipline(disciplineData);
                return StatusCode(200);
            }
            catch (NoRecord)
            {
                return StatusCode(409); // Conflict
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }

        [HttpDelete("discplines/{disciplineID}")]
        public async Task<ActionResult> DeleteDiscipline(int userID, int disciplineID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            try
            {
                await discipline.DeleteDiscipline(userID, disciplineID);
                return StatusCode(200);
            }
            catch (NoRecord e)
            {
                logger.Info(e, "Нет дисциплины в БД");
                return StatusCode(409); // Conflict
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }

        [HttpGet("disciplines/{disciplineID}/tasks")]
        public async Task<ActionResult<List<TaskData>>> GetDTasks(int userID, int disciplineID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            List<TaskData>? tasks = null;
            try
            {
                tasks = await discipline.GetDTasks(userID, disciplineID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (tasks != null)
                return tasks;
            else
                return new List<TaskData>();
        }

        [HttpGet("tasks")]
        public async Task<ActionResult<List<TaskData>>> GetTasks(int userID, string? dateFrom, string? dateTo)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            List<TaskData>? tasks = null;
            try
            {
                if (dateFrom != null && dateTo != null)
                    tasks = await discipline.GetTasks(userID, DateTime.Parse(dateFrom), DateTime.Parse(dateTo));
                else
                    tasks = await discipline.GetTasks(userID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (tasks != null)
                return tasks;
            else
                return new List<TaskData>();
        }

        [HttpGet("tasks/{taskID}")]
        public async Task<ActionResult<TaskData>> GetTask(int userID, int taskID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            TaskData? task = null;
            try
            {
                task = await discipline.GetTask(userID, taskID);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
            if (task != null)
                return task;
            else
                return StatusCode(404);
        }

        [HttpPost("tasks")]
        public async Task<ActionResult> CreateTask(int userID, TaskData task)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID || userID != task.UserID)
                return StatusCode(403);
            try
            {
                await discipline.CreateTask(task);
                return StatusCode(201);
            }
            catch (ExistingName)
            {
                return StatusCode(409);
            }
            catch (NoRecord)
            {
                return StatusCode(409);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }

        [HttpPut("tasks/{taskID}")]
        public async Task<ActionResult> UpdateTask(int userID, int taskID, TaskData taskData)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID || userID != taskData.UserID)
                return StatusCode(403);
            try
            {
                await discipline.UpdateTask(taskData);
                return StatusCode(200);
            }
            catch (NoRecord)
            {
                return StatusCode(409);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }

        [HttpDelete("tasks/{taskID}")]
        public ActionResult DeleteTask(int userID, int taskID)
        {
            int responceUserID = Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (responceUserID != userID)
                return StatusCode(403);
            try
            {
                discipline.DeleteTask(userID, taskID);
                return StatusCode(200);
            }
            catch (NoRecord e)
            {
                return StatusCode(409);
            }
            catch (NoDBConnection e)
            {
                logger.Error(e, "Отсутсвует соедиение с БД");
                return StatusCode(503);
            }
            catch (NoAccessRight e)
            {
                logger.Error(e, "Нет прав доступа к базе данных");
                return StatusCode(403);
            }
            catch (Exception e)
            {
                logger.Error(e, "Непредвиденная ошибка");
                return StatusCode(500);
            }
        }
    }
}
