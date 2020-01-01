public enum StageDataType
{
    ReadFromScene,
    ReadFromCsv,
}

public enum Direction
{
    Left,
    Right,
    Down,
    Up,
}

public enum BlockType
{
    None,
    Dynamic,
    Static,
    StartPoint,
    EndPoint
}

public enum DynamicBlockType
{
    None,
    Vertical,
    Horizontal,
    UpAndRight,
    UpAndLeft,
    DownAndRight,
    DownAndLeft,
}