using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BlendGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(150, 200);

    public BlendGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateEntryPointNode());
    }

    private Port GeneratePort(ComboNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private ComboNode GenerateEntryPointNode()
    {
        var node = new ComboNode
        {
            title = "Start",
            guid = Guid.NewGuid().ToString(),
            attackChain = "A",
            entryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output, Port.Capacity.Single);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));

        return node;
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateComboNode(nodeName));
    }

    public ComboNode CreateComboNode(string nodeName)
    {
        var comboNode = new ComboNode
        {
            title = nodeName,
            guid = Guid.NewGuid().ToString(),
            attackChain = ""
        };

        var inputPort = GeneratePort(comboNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        comboNode.inputContainer.Add(inputPort);
        comboNode.RefreshExpandedState();
        comboNode.RefreshPorts();
        comboNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return comboNode;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }
}
