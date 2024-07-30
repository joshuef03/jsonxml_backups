import os


class Main:
    @staticmethod
    def print_hello_world():
        print("Hello, World!")

    @staticmethod
    def print_current_directory():
        print("Current Directory:", os.getcwd())


if __name__ == "__main__":
    Main.print_hello_world()
    Main.print_current_directory()
