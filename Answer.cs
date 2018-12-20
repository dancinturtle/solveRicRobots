using System;
using System.Collections.Generic;

namespace RicRobots
{
    class Answer
    {
     private Queue<string> robotColors;
     public Queue<string> RobotColors
     {
         get => robotColors;
     }
     private int totalSteps;
     public int TotalSteps
     {
         get => totalSteps;
     }
     private Queue<int> robotSteps;
     public Queue<int> RobotSteps
     {
         get => robotSteps;
     }
     private Queue<int[]> robotDestinations;
     public Queue<int[]> RobotDestinations
    {
        get => robotDestinations;
    }
     private Queue<Dictionary<Node, Object[]>> trackers;
     public Queue<Dictionary<Node, Object[]>> Trackers
     {
         get => trackers;
     }
      public Answer(string robotColor, int robotSteps, int[] robotDestination, Dictionary<Node, Object[]> robotTracker)
      {
          this.robotColors = new Queue<string>();
          this.robotColors.Enqueue(robotColor);
          this.totalSteps = robotSteps;
          this.robotSteps = new Queue<int>();
          this.robotSteps.Enqueue(robotSteps);
          this.robotDestinations = new Queue<int[]>();
          this.robotDestinations.Enqueue(robotDestination);
          this.trackers = new Queue<Dictionary<Node, Object[]>>();
          this.trackers.Enqueue(robotTracker);
      }
      public void addRobot(string robotColor, int robotSteps, int[] robotDestination, Dictionary<Node, Object[]> robotTracker)
      {
          this.robotColors.Enqueue(robotColor);
          this.totalSteps += robotSteps;
          this.robotSteps.Enqueue(robotSteps);
          this.robotDestinations.Enqueue(robotDestination);
          this.trackers.Enqueue(robotTracker);
      }

    }

}
