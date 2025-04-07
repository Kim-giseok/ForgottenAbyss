public class BTMachine
{
    private EnemyController controller;
    private BlackBoard blackBoard = new();

    private bool isLooping = false;
    private Node rootNode;
    private Node currNode;

    public BTMachine(EnemyController controller)
    {
        this.controller = controller;
    }
    
    public void Run()
    {
        if (!isLooping) return;
        currNode.Update();
    }

    public void SetState(Node newNode)
    {
        isLooping = false;
        
        currNode?.End();
        currNode = newNode;
        currNode.Start();
        
        isLooping = true;
    }
}