import 'Arvy.dart'

void main() {
    var a = new ActionResponseViewModel()
        ..ResponseType = ActionResponseViewModel.Error
        ..Message = "Bug: Application need configured first.";

    print("${a.ResponseType} ==> ${a.Message}");
}