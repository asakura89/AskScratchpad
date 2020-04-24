IF ISNULL(NULLIF('a', ''), 0) <> 0
BEGIN
    SELECT 'b'
END
