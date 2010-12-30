CREATE VIEW [dbo].[PrFStatistics]
AS
SELECT
PrFActionLog.ActionKey 'Action'
, (
	SELECT AVG(X) * AVG(PrFActionLog.Milliseconds) FROM
	(
		SELECT COUNT(T.ID) [X] FROM PrFActionLog T
		WHERE T.ActionKey = PrFActionLog.ActionKey
		GROUP BY T.ActionKey, MONTH(T.Date)
	) T
) [Stress Ratio]
, AVG(PrFActionLog.Milliseconds) 'Time(Avg)'
, (
	SELECT AVG(X) FROM
	(
		SELECT COUNT(T.ID) [X] FROM PrFActionLog T
		WHERE T.ActionKey = PrFActionLog.ActionKey
		GROUP BY T.ActionKey, MONTH(T.Date)
	) T
) 'Executions(Avg)'
, (
	SELECT ISNULL(AVG(X), 0) FROM
	(
		SELECT COUNT(T.ID) [X] FROM PrFExceptionLog T
		INNER JOIN PrFActionLog AL ON AL.ID = T.ActionLogID
		WHERE AL.ActionKey = PrFActionLog.ActionKey
		GROUP BY AL.ActionKey, MONTH(AL.Date)
	) T
) 'Exceptions(Avg)'
, SUM(PrFActionLog.Milliseconds) 'Time(Total)'
, COUNT(PrFActionLog.ID) 'Executions(Total)'
, COUNT(PrFExceptionLog.ID) 'Exceptions(Total)'
FROM PrFActionLog
LEFT JOIN PrFExceptionLog ON PrFExceptionLog.ActionLogID = PrFActionLog.ID
GROUP BY PrFActionLog.ActionKey
