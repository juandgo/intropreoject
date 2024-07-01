using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainSGridAnimator : SGridAnimator
{
    protected override Coroutine DetermineAndStartMoving(SMove move, STile[,] grid, Movement m, bool playSound)
    {
        int diff = m.endLoc.y - m.startLoc.y;
        bool onSameLevel = (Mathf.Abs(diff) <= 1); 

        STile tile = Player.GetInstance().GetSTileUnderneath();

        bool isPlayerOnStile = (tile != null && tile.islandId == grid[m.startLoc.x, m.startLoc.y].islandId);
        if(!onSameLevel && isPlayerOnStile)
        {
            if(diff > 0)
                CameraZoom.MoveUp(movementDurationMultiplier);
            else
                CameraZoom.MoveDown(movementDurationMultiplier);
        }
        return StartCoroutine(StartMovingAnimation(grid[m.startLoc.x, m.startLoc.y], m, move, animate:onSameLevel, playSound: playSound));
    }

    protected override void EffectOnMoveStart(SMove move, Movement movement, Transform root, STile tile, float currMoveDuration, bool playSound)
    {
        base.EffectOnMoveStart(move, movement, root, tile, currMoveDuration, playSound);
        if((move as SMoveMountainSwap).isLayerSwap && tile.isTileActive) {
            if(Player.GetInstance().GetSTileUnderneath() == tile) {
                //Player on tile, dither world
                ((MountainSTile) tile).AnimateBorderTileDither(movementDurationMultiplier * move.duration);
            }
            else {
                //Player not on tile, dither tile
                ((MountainSTile) tile).AnimateTileDither(movementDurationMultiplier * move.duration);
            }
        }
    }
}
