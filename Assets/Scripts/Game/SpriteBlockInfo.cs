using System.Collections.Generic;

public class SpriteBlockInfo : Singleton<SpriteBlockInfo>
{
    public enum SpriteBlock
    {
        Null,

        Normal,

        DynamicVertical,
        DynamicHorizontal,
        DynamicUpAndLeft,
        DynamicUpAndRight,
        DynamicDownAndLeft,
        DynamicDownAndRight,

        StaticVertical,
        StaticHorizontal,
        StaticUpAndLeft,
        StaticUpAndRight,
        StaticDownAndLeft,
        StaticDownAndRight,

        StartPointUp,
        StartPointDown,
        StartPointLeft,
        StartPointRight,

        EndPointUp,
        EndPointDown,
        EndPointLeft,
        EndPointRight,

        Count,
    }

    private static Dictionary<SpriteBlock, string> keyValuePairs = new Dictionary<SpriteBlock, string> ()
    {
        { SpriteBlock.Normal, "block_n" },

        { SpriteBlock.DynamicVertical, "block_v" },
        { SpriteBlock.DynamicHorizontal, "block_h" },
        { SpriteBlock.DynamicUpAndLeft, "block_ul" },
        { SpriteBlock.DynamicUpAndRight, "block_ur" },
        { SpriteBlock.DynamicDownAndLeft, "block_dl" },
        { SpriteBlock.DynamicDownAndRight, "block_dr" },

        { SpriteBlock.StaticVertical, "block_v_fix" },
        { SpriteBlock.StaticHorizontal, "block_h_fix" },
        { SpriteBlock.StaticUpAndLeft, "block_ul_fix" },
        { SpriteBlock.StaticUpAndRight, "block_ur_fix" },
        { SpriteBlock.StaticDownAndLeft, "block_dl_fix" },
        { SpriteBlock.StaticDownAndRight, "block_dr_fix" },

        { SpriteBlock.StartPointUp, "block_u_start" },
        { SpriteBlock.StartPointDown, "block_d_start" },
        { SpriteBlock.StartPointLeft, "block_l_start" },
        { SpriteBlock.StartPointRight, "block_r_start" },

        { SpriteBlock.EndPointUp, "block_u_goal" },
        { SpriteBlock.EndPointDown, "block_d_goal" },
        { SpriteBlock.EndPointLeft, "block_l_goal" },
        { SpriteBlock.EndPointRight, "block_r_goal" },
    };

    public static string GetSpriteName(SpriteBlock spriteBlock)
    {
        if(keyValuePairs.ContainsKey(spriteBlock))
        {
            return keyValuePairs[spriteBlock];
        }

        return string.Empty;
    }

    public static SpriteBlock GetSpriteBlockEnum(string str)
    {
        for(int i=0; i<(int)SpriteBlock.Count; i++)
        {
            var spriteBlock = (SpriteBlock) i;
            if(GetSpriteName(spriteBlock).Equals(str))
            {
                return spriteBlock;
            }
        }

        return SpriteBlock.Null;
    }


    public static BlockType GetBlockType (SpriteBlock spriteBlockEnum)
    {
        BlockType returnBlockType = BlockType.Null;

        switch(spriteBlockEnum)
        {
            case SpriteBlock.Normal:
            case SpriteBlock.DynamicVertical:
            case SpriteBlock.DynamicHorizontal:
            case SpriteBlock.DynamicUpAndLeft:
            case SpriteBlock.DynamicUpAndRight:
            case SpriteBlock.DynamicDownAndLeft:
            case SpriteBlock.DynamicDownAndRight:
                returnBlockType = BlockType.Dynamic;
                break;

            case SpriteBlock.StaticVertical:
            case SpriteBlock.StaticHorizontal:
            case SpriteBlock.StaticUpAndLeft:
            case SpriteBlock.StaticUpAndRight:
            case SpriteBlock.StaticDownAndLeft:
            case SpriteBlock.StaticDownAndRight:
                returnBlockType = BlockType.Static;
                break;

            case SpriteBlock.StartPointUp:
            case SpriteBlock.StartPointDown:
            case SpriteBlock.StartPointLeft:
            case SpriteBlock.StartPointRight:
                returnBlockType = BlockType.StartPoint;
                break;

            case SpriteBlock.EndPointUp:
            case SpriteBlock.EndPointDown:
            case SpriteBlock.EndPointLeft:
            case SpriteBlock.EndPointRight:
                returnBlockType = BlockType.EndPoint;
                break;
        }

        return returnBlockType;
    }

    public static BlockDirectionType GetBlockDirectionType (SpriteBlock spriteBlockEnum)
    {
        BlockDirectionType returnBlockType = BlockDirectionType.Null;

        switch (spriteBlockEnum)
        {
            case SpriteBlock.DynamicVertical:
            case SpriteBlock.StaticVertical:
                returnBlockType = BlockDirectionType.Vertical;
                break;

            case SpriteBlock.DynamicHorizontal:
            case SpriteBlock.StaticHorizontal:
                returnBlockType = BlockDirectionType.Horizontal;
                break;

            case SpriteBlock.StaticUpAndLeft:
            case SpriteBlock.DynamicUpAndLeft:
                returnBlockType = BlockDirectionType.UpAndLeft;
                break;

            case SpriteBlock.StaticUpAndRight:
            case SpriteBlock.DynamicUpAndRight:
                returnBlockType = BlockDirectionType.UpAndRight;
                break;

            case SpriteBlock.StaticDownAndLeft:
            case SpriteBlock.DynamicDownAndLeft:
                returnBlockType = BlockDirectionType.DownAndLeft;
                break;

            case SpriteBlock.StaticDownAndRight:
            case SpriteBlock.DynamicDownAndRight:
                returnBlockType = BlockDirectionType.DownAndRight;
                break;

            case SpriteBlock.StartPointUp:
            case SpriteBlock.EndPointUp:
                returnBlockType = BlockDirectionType.Up;
                break;

            case SpriteBlock.StartPointDown:
            case SpriteBlock.EndPointDown:
                returnBlockType = BlockDirectionType.Down;
                break;

            case SpriteBlock.StartPointLeft:
            case SpriteBlock.EndPointLeft:
                returnBlockType = BlockDirectionType.Left;
                break;

            case SpriteBlock.StartPointRight:
            case SpriteBlock.EndPointRight:
                returnBlockType = BlockDirectionType.Right;
                break;
        }

        return returnBlockType;
    }
}