using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static public DataManager instance;
    public SortedList<int, Command> recordedCommands = new SortedList<int, Command>();
    public List<SerializableCommand> serializableCommands = new List<SerializableCommand>();

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveReplayData()
    {
        CommandsToSerializableCommand();

        int stageIndex = GameManager.instance.stageIndex;
        string folderPath = Path.Combine(Application.persistentDataPath, "Replays");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = $"Stage_{stageIndex}_replay.json";
        string fullPath = Path.Combine(folderPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log($"���� ���÷��� ���� ����: {fullPath}");
        }

        CommandWrapper wrapper = new CommandWrapper { commands = serializableCommands };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(fullPath, json);
        Debug.Log($"���÷��� ���� �Ϸ�: {fullPath}");

        serializableCommands.Clear();
        recordedCommands.Clear();
    }

    public void CommandsToSerializableCommand()
    {
        List<SerializableCommand> serializableCommands = DataManager.instance.serializableCommands;
        foreach (var kvp in DataManager.instance.recordedCommands)
        {
            SerializableCommand sc = new SerializableCommand
            {
                frame = kvp.Key,
                commandType = kvp.Value.command.ToString() // �Ǵ� Ŀ�ǵ帶�� �̸� ���� ��� ����
            };
            serializableCommands.Add(sc);
        }
    }

    public void LoadReplayData()
    {
        int stageIndex = GameManager.instance.stageIndex;
        string path = Path.Combine(Application.persistentDataPath, "Replays", $"Stage_{stageIndex}_replay.json");
        string json = File.ReadAllText(path);

        if (!string.IsNullOrEmpty(json))
        {
            CommandWrapper wrapper = JsonUtility.FromJson<CommandWrapper>(json);
            recordedCommands.Clear();

            foreach (var sc in wrapper.commands)
            {
                Command cmd = ConvertToCommand(sc.commandType);
                recordedCommands.Add(sc.frame, cmd);
            }
        }
    }

    private Command ConvertToCommand(string commandType)
    {
        switch (commandType)
        {
            case "JUMP":
                return new JumpCommand();
            case "ROTATE_LEFT":
                return new RotateCommand(CommandType.ROTATE_LEFT);
            case "ROTATE_RIGHT":
                return new RotateCommand(CommandType.ROTATE_RIGHT);
            case "RECOVER_DIR":
                return new RecoverDirCommand();
            default:
                Debug.LogWarning($"Unknown command: {commandType}");
                return null;
        }
    }

}
