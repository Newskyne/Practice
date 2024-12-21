USE [Practice]
GO

/****** Object:  Trigger [dbo].[trg_ApplyDiscount]    Script Date: 20.12.2024 16:19:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_ApplyDiscount]
ON [dbo].[order]
AFTER INSERT
AS
BEGIN
    DECLARE @orderId INT;
    DECLARE @userId INT;
    DECLARE @sum DECIMAL(18, 2);
    DECLARE @totalPaid DECIMAL(18, 2);
    DECLARE @discount DECIMAL(18, 2);

    -- Получаем данные из вставленной строки
    SELECT @orderId = id, @userId = user_id, @sum = sum
    FROM inserted;

    -- Рассчитываем общую сумму оплаченной продукции для пользователя
    SELECT @totalPaid = SUM(sum)
    FROM [order]
    WHERE user_id = @userId;

    -- Рассчитываем скидку
    SET @discount = dbo.CalculateDiscount(@totalPaid);

    -- Применяем скидку к сумме
    UPDATE [order]
    SET sum = @sum * (1 - @discount)
    WHERE id = @orderId;
END;
GO

ALTER TABLE [dbo].[order] ENABLE TRIGGER [trg_ApplyDiscount]
GO


