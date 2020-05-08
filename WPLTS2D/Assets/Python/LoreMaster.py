
class LoreMaster:
	def __init__(self, datapath):
		self.datapath = datapath
		print("Lore Master Initiated.")
	def GetTest(self):
		with open(self.datapath+"testfile.txt", "r") as f:
			return f.read()
		return "error?"