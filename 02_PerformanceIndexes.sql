USE DapperMegaSalesAnalyticsDb;
GO

/* =========================================================
   CORE PAGINATION INDEX
   Main listing: WHERE IsDeleted = 0 ORDER BY SalesTransactionId DESC
   ========================================================= */

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_IsDeleted_Id_Desc'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_IsDeleted_Id_Desc
    ON SalesTransactions(IsDeleted, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        TotalPrice,
        OrderStatus,
        PaymentMethod,
        SalesChannel,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO


/* =========================================================
   ADVANCED FILTER INDEXES
   These support city/category/status/payment/channel/date/price filters
   ========================================================= */

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_City'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_City
    ON SalesTransactions(IsDeleted, City, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        TotalPrice,
        OrderStatus,
        PaymentMethod,
        SalesChannel,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_Category'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_Category
    ON SalesTransactions(IsDeleted, ProductCategory, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        Quantity,
        UnitPrice,
        TotalPrice,
        OrderStatus,
        PaymentMethod,
        SalesChannel,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_Status'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_Status
    ON SalesTransactions(IsDeleted, OrderStatus, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        TotalPrice,
        PaymentMethod,
        SalesChannel,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_Payment'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_Payment
    ON SalesTransactions(IsDeleted, PaymentMethod, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        TotalPrice,
        OrderStatus,
        SalesChannel,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_Channel'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_Channel
    ON SalesTransactions(IsDeleted, SalesChannel, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        TotalPrice,
        OrderStatus,
        PaymentMethod,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_OrderDate'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_OrderDate
    ON SalesTransactions(IsDeleted, OrderDate, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        TotalPrice,
        OrderStatus,
        PaymentMethod,
        SalesChannel,
        DeliveryDay,
        CustomerAge
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Filter_TotalPrice'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Filter_TotalPrice
    ON SalesTransactions(IsDeleted, TotalPrice, SalesTransactionId DESC)
    INCLUDE
    (
        OrderNumber,
        CustomerFullName,
        CustomerEmail,
        City,
        Country,
        ProductName,
        ProductCategory,
        Quantity,
        UnitPrice,
        OrderStatus,
        PaymentMethod,
        SalesChannel,
        OrderDate,
        DeliveryDay,
        CustomerAge
    );
END
GO


/* =========================================================
   DASHBOARD ANALYTICS INDEXES
   These support completed revenue, category, city, channel, monthly charts
   ========================================================= */

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Dashboard_MonthlyRevenue'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Dashboard_MonthlyRevenue
    ON SalesTransactions(IsDeleted, OrderStatus, OrderDate)
    INCLUDE (TotalPrice);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Dashboard_CategoryRevenue'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Dashboard_CategoryRevenue
    ON SalesTransactions(IsDeleted, OrderStatus, ProductCategory)
    INCLUDE (TotalPrice);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Dashboard_CityRevenue'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Dashboard_CityRevenue
    ON SalesTransactions(IsDeleted, OrderStatus, City)
    INCLUDE (TotalPrice);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Dashboard_ChannelRevenue'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Dashboard_ChannelRevenue
    ON SalesTransactions(IsDeleted, OrderStatus, SalesChannel)
    INCLUDE (TotalPrice);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_SalesTransactions_Dashboard_PaymentStatus'
      AND object_id = OBJECT_ID('SalesTransactions')
)
BEGIN
    CREATE INDEX IX_SalesTransactions_Dashboard_PaymentStatus
    ON SalesTransactions(IsDeleted, PaymentMethod, OrderStatus)
    INCLUDE (TotalPrice);
END
GO


/* =========================================================
   MAINTENANCE
   Update statistics after large insert + index creation
   ========================================================= */

UPDATE STATISTICS SalesTransactions;
GO