# RepositoryTools
### DAL Repository ef/efCore (Temporary Tables, SoftDelete, BaseEntity)

Реализация репозитория для удобного доступа к хранилищу данных, получение и изменение сущности предметной области.

Представлены реализации репозитория с помощью EF и EF Core.

Для работы с BaseEntity реализованы декораторы, с помощью которых происходит автоматический учет
и изменение полей (SoftDelete, ChangeDate, ChangedBy).

Реализована возможность работы с временными таблицами для улучшения производительности. 
Так же добавлены методы для блочной обработки данных (использование библиотек для EF).
