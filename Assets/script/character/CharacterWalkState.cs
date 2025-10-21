using System.Collections;
using System.Collections.Generic;
using Fungus;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterWalkState : CharacterState
{
    private int tileIndex = 0;

    public CharacterWalkState(CharacterStateMachine _stateMachine, NpcController _npc, string _animBoolName) : base(_stateMachine, _npc, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        //判断导航网格是否扫描完成
        if (AstarPath.active.isScanning) return;

        //增加60帧延迟
        if (Time.frameCount < 60) return;

        if (npc.attackTarget != null)
        {
            GameObject target = npc.attackTarget;

            NavGraph[] graphs = AstarPath.active.data.graphs;
            GridGraph graph = graphs[npc.gridGraphIndex] as GridGraph;
            GraphNode node = graph.GetNearest(target.transform.position).node;
            if (node == null || !node.Walkable)
            {
                Debug.Log("超出范围，返回 " + npc.prePosition);
                npc.destinationSetter.target = null;
                npc.seeker.CancelCurrentPathRequest();
                npc.seeker.StartPath(npc.transform.position, npc.prePosition);
                npc.SetAttackTarget(null);
                return;
            }
            npc.seeker.CancelCurrentPathRequest();
            npc.destinationSetter.target = target.transform;
            if (Vector2.Distance(npc.transform.position, target.transform.position) < 5f)
            {
                npc.seeker.CancelCurrentPathRequest();
                npc.destinationSetter.target = null;
                stateMachine.ChangeState(npc.attackState);
                return;
            }
        }
        else
        {
            List<Vector3Int> tiles = npc.tiles;

            if (tiles.Count == 0)
            {
                if (npc.checkPoints != null && -1 != npc.index)
                {
                    int pointLength = npc.checkPoints.Length;
                    if (pointLength == 0)
                        return;

                    CheckPoint target = npc.checkPoints[npc.index];
                    NavGraph[] graphs = AstarPath.active.data.graphs;
                    GridGraph graph = graphs[npc.gridGraphIndex] as GridGraph;
                    GraphNode node = graph.GetNearest(target.transform.position).node;
                    if (node == null || !node.Walkable)
                    {
                        Debug.Log(npc.index + " point 不可达");
                        npc.AddIndex(pointLength);
                        return;
                    }

                    npc.seeker.CancelCurrentPathRequest();
                    npc.destinationSetter.target = target.transform;

                    if (Vector2.Distance(npc.transform.position, target.transform.position) < 0.5f)
                    {
                        npc.AddIndex(pointLength);
                        stateMachine.ChangeState(npc.idleState);
                        return;
                    }
                }
                else
                {
                    npc.destinationSetter.target = npc.transform;
                }
            }
            else
            {
                if (tileIndex >= tiles.Count)
                {
                    tileIndex = 0;
                    npc.AddIndex();
                    npc.tiles.Clear();
                }
                else
                {
                    Vector3 worldPosition = npc.tilemap.CellToWorld(tiles[tileIndex]);

                    NavGraph[] graphs = AstarPath.active.data.graphs;
                    GridGraph graph = graphs[npc.gridGraphIndex] as GridGraph;
                    GraphNode node = graph.GetNearest(worldPosition).node;
                    if (node == null || !node.Walkable)
                    {
                        tileIndex++;
                        return;
                    }

                    npc.destinationSetter.target = null;
                    npc.seeker.CancelCurrentPathRequest();
                    npc.seeker.StartPath(npc.transform.position, worldPosition);

                    if (Vector2.Distance(npc.transform.position, worldPosition) < 0.5f)
                    {
                        tileIndex++;
                        stateMachine.ChangeState(npc.plantState);
                        return;
                    }
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
