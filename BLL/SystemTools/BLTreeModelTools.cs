using CyberneticCode.Web.Mvc.UIControls.Tree;
using Model.ToolsModels.Tree;
using System.Collections.Generic;
using System.Linq;

namespace BLL.SystemTools
{
    public class BLTreeModelTools
    {
        IEnumerable<ITmNode> rawNodeList;

        public List<JsTreeNode> GetTreeModel(IEnumerable<ITmNode> treeModelList)
        {
            rawNodeList = treeModelList;

            var jsTreeNodeList = new List<JsTreeNode>();

            var rootNode = rawNodeList.FirstOrDefault(r => r.ParentId == null);
            var children = rawNodeList.Where(c => c.ParentId == rootNode.Id).ToList();
            var jsRootNode = new JsTreeNode
            {
                id = rootNode.Id.ToString(),
                text = rootNode.Name,
                icon = "",
                a_attr = { href = "#" },
                additionalData = rootNode.AdditionalData
            };

            jsRootNode.state.opened = true;

            GenereateTreeModel(jsRootNode, children);

            jsTreeNodeList.Add(jsRootNode);

            return jsTreeNodeList;
        }
        void GenereateTreeModel(JsTreeNode rootNode, List<ITmNode> children)
        {

            if (children.Count == 0) return;

            foreach (var node in children)
            {
                var jsNode = new JsTreeNode
                {
                    id = node.Id.ToString(),
                    text = node.Name,
                    icon = "",
                    a_attr = { href = "#" },
                    additionalData = node.AdditionalData
                };

                jsNode.state.opened = false;

                rootNode.children.Add(jsNode);

                GenereateTreeModel(jsNode, rawNodeList.Where(c => c.ParentId == node.Id).ToList());

            }

        }
    }
}
