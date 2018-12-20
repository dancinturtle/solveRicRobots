using System;
using System.Collections.Generic;

namespace RicRobots
{
    class Answer
    {
      private Board secondBoard;
      public Board SecondBoard
      {
          get => secondBoard;
      }
      private string helperColor;
      public string HelperColor
      {
          get => helperColor;
      }
      private Node helper;
      public Node Helper
      {
          get => helper;
      }
      private Node helperDestination;
      public Node HelperDestination
      {
          get => helperDestination;
      }
      private string targetRobotColor;
      public string TargetRobotColor
      {
          get => targetRobotColor;
      }

      private Node targetRobot;
      public Node TargetRobot
      {
          get => targetRobot;
      }
      private Node destination;
      public Node Destination
      {
          get => destination;
      }
      private Dictionary<Node, Object[]> helperPath;
      public Dictionary<Node, Object[]>HelperPath
      {
          get => helperPath;
      }
      private Dictionary<Node, Object[]> targetRobotPath;
      public Dictionary<Node, Object[]>TargetRobotPath
      {
          get => targetRobotPath;
      }
      private int totalSteps;
      public int TotalSteps
      {
          get => totalSteps;
      }
      private int helperSteps;
      public int HelperSteps
      {
          get => helperSteps;
      }
      private int targetRobotSteps;
      public int TargetRobotSteps
      {
          get => targetRobotSteps;
      }
      public Answer(string helperColor, Node helper, Node helperDestination, string targetColor, Node targetRobot, Node destination, Dictionary<Node, Object[]> helperPath, Dictionary<Node, Object[]> targetRobotPath, int totalSteps, int helperSteps, Board board, int targetRobotSteps )
      {
          this.helperColor = helperColor;
          this.helper = helper;
          this.helperDestination = helperDestination;
          this.targetRobotColor = targetColor;
          this.targetRobot = targetRobot;
          this.destination = destination;
          this.helperPath = helperPath;
          this.targetRobotPath = targetRobotPath;
          this.totalSteps = totalSteps;
          this.helperSteps = helperSteps;
          this.targetRobotSteps = targetRobotSteps;
          this.secondBoard = board;
      }

    }

}
