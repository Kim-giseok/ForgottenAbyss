public class BTMachine
{
    private EnemyController controller;
    private BlackBoard blackBoard = new();

    private bool isLooping = false;
    private Node rootNode;
    public Node currNode { get; private set; }

    public BTMachine(EnemyController controller)
    {
        this.controller = controller;
    }
    
    public void Run()
    {
        if (!isLooping) return;
        currNode.Update();
    }

    public void SetNode(Node newNode)
    {
        isLooping = false;
        
        currNode?.End();
        currNode = newNode;
        currNode.Start();
        
        isLooping = true;
    }

    public void OnAnimatedEvent(bool isFire)
    {
        currNode?.OnAnimatedEvent(isFire);
    }

    public void Define(Node newNode)
    {
        rootNode = new RootNode(newNode);
        currNode = rootNode;
    }
}