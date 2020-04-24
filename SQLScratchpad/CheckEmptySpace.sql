
SELECT
    ' ' '1',
    '  ' '2',
    '   ' '3',
    '          ' '10',
    LTRIM(RTRIM(' ')) TRIM_1,
    LTRIM(RTRIM('  ')) TRIM_2,
    LTRIM(RTRIM('   ')) TRIM_3,
    LTRIM(RTRIM('          ')) TRIM_10,
    NULLIF(' ', ' ') NULLIF_1,
    NULLIF('  ', ' ') NULLIF_2,
    NULLIF(LTRIM(RTRIM(' ')), ' ') NULLIF_TRIM_1,
    NULLIF(LTRIM(RTRIM('  ')), ' ') NULLIF_TRIM_2,
    NULLIF(LTRIM(RTRIM('   ')), ' ') NULLIF_TRIM_3,
    NULLIF(LTRIM(RTRIM('          ')), ' ') NULLIF_TRIM_10
