using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum
{
    public enum StatusCode
    {
        Ok = 0,
        ObjectNotFound = 1,
        InvalidRequest = 2,
        IncorrectRequest = 3,
        UndefinedError = 4,
        UserAlreadyExists = 5,
        /*
        0 — ОК,
        1 — запрашиваемый объект не найден,
        2 — невалидный запрос (например, не парсится json),
        3 — некорректный запрос (семантически),
        4 — неизвестная ошибка.
        5 — такой юзер уже существует
        */
    }
}
