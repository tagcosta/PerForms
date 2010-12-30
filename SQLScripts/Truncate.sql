delete from PrFExceptionLog
DBCC CHECKIDENT (PrFExceptionLog, RESEED, 0)

delete from PrFActionLogParameter
DBCC CHECKIDENT (PrFActionLogParameter, RESEED, 0)

delete from PrFActionLog
DBCC CHECKIDENT (PrFActionLog, RESEED, 0)
