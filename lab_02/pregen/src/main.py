from fastapi import FastAPI
from pydantic import *


class Discipline(BaseModel):
    Name: str
    Description: str
    Summarize: int

class Task(BaseModel):
    Name: str
    DName: str
    Discription: str
    Finished: bool
    Cost: int
    Data: str

class Calendar(BaseModel):
    Day: int
    Message: str

class Mesg(BaseModel):
    string: str


app = FastAPI()


# Calendars
@app.get("/{user}/calendardfbsd", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_calendars(user: str, login: str, password: str):
    return 0

# получить данные о всех оповещаниях
@app.get("/{user}/calendar", response_model=list[Calendar], responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_calendars(user: str):
    return 0

# получить данные об оповещании
@app.get("/{user}/calendar/{days}", response_model=Calendar, responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_calendar(user: str, days: int):
    return 0

# удалить оповещание
@app.delete("/{user}/calendar/{days}", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def delete_calendar(user: str, days: int):
    return 0

# обновить оповещание
@app.post("/{user}/calendar/{days}", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def post_calendar(user: str, days: int, calendar: Calendar):
    return 0

# создать оповещание
@app.patch("/{user}/calendar", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def patch_calendar(user: str, calendar: Calendar):
    return 0


# Disciplines

# получить данные о всех дисциплинах
@app.get("/{user}/discipline", response_model=list[Discipline], responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_disciplines(user: str):
    return 0

# получить данные о дисциплине
@app.get("/{user}/discipline/{name}", response_model=Discipline, responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_discipline(user: str, name: str):
    return 0

# получить данные о всех задачах дисциплины
@app.get("/{user}/discipline/{name}/tasks", response_model=list[Task], responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_Dtasks(user: str, name: str):
    return 0

# удалить дисциплину
@app.delete("/{user}/discipline/{name}", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def delete_discipline(user: str, name: str):
    return 0

# обновить дисциплину
@app.post("/{user}/discipline/{name}", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def post_discipline(user: str, name: str, discipline: Discipline):
    return 0

# создать дисциплину
@app.patch("/{user}/discipline", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def patch_discipline(user: str, discipline: Discipline):
    return 0

# Tasks

# получить данные о всех задачах
@app.get("/{user}/task", response_model=list[Task], responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_tasks(user: str):
    return 0

# получить данные о задаче
@app.get("/{user}/task/{name}", response_model=Task, responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_task(user: str, name: str):
    return 0

# удалить задачу
@app.delete("/{user}/task/{name}", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def delete_task(user: str, name: str):
    return 0

# обновить задачу
@app.post("/{user}/task/{name}", responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def post_task(user: str, name: str, task: Task):
    return 0

# создать задачу
@app.patch("/{user}/task")
def patch_task(user: str, task: Task):
    return 0

# создать пользователя


# авторизоваться


# получить календарь задач
@app.get("/{user}/taskstable", response_model=list[Task], responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_tasktable(user: str):
    return 0

# получить актуальные оповещания
@app.get("/{user}/notification", response_model=list[Calendar], responses={403: {'model': Mesg}, 404: {'model': Mesg}})
def get_notification(user: str):
    return 0
