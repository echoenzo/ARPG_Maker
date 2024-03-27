using Godot;
using System;

public partial class SoftCollision : Area2D
{
    private Vector2 putVector = Vector2.Zero;//推开的向量
    public bool IsCollisioning()
    {
        var areas = GetOverlappingAreas();//获取重叠数组
        return areas.Count > 0;
    }
    public Vector2 GetPutVector()
    {
        var areas = GetOverlappingAreas();//获取重叠数组

        if (IsCollisioning())
        {
            var area = areas[0];//只检测数组中的第一个  
            putVector = area.GlobalPosition.DirectionTo(GlobalPosition);//从重叠区域指向碰撞点的向量

        }
        return putVector;
    }
}
