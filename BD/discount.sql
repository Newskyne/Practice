USE [Practice]
GO

/****** Object:  UserDefinedFunction [dbo].[CalculateDiscount]    Script Date: 20.12.2024 16:19:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[CalculateDiscount](@totalPaid DECIMAL(18, 2))
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @discount DECIMAL(18, 2) = 0;

    IF @totalPaid >= 10000 AND @totalPaid < 50000
    BEGIN
        SET @discount = 0.05;
    END
    ELSE IF @totalPaid >= 50000 AND @totalPaid < 300000
    BEGIN
        SET @discount = 0.10;
    END
    ELSE IF @totalPaid >= 300000
    BEGIN
        SET @discount = 0.15;
    END

    RETURN @discount;
END;
GO


