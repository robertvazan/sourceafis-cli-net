# This script generates and updates project configuration files.

# Run this script with rvscaffold in PYTHONPATH
import rvscaffold as scaffold

class Project(scaffold.Net):
    def script_path_text(self): return __file__
    def root_namespace(self): return 'SourceAFIS.Cli'
    def pretty_name(self): return 'SourceAFIS CLI for .NET'
    def nuget_title(self): return 'SourceAFIS CLI'
    def repository_name(self): return 'sourceafis-cli-net'
    def inception_year(self): return 2021
    def nuget_description(self): return 'Command-line interface for SourceAFIS.'
    def nuget_tags(self): return 'fingerprint; biometrics; authentication; sourceafis'
    def project_status(self): return self.stable_status()
    def md_description(self): return '''\
        SourceAFIS CLI for .NET is a command-line interface to [SourceAFIS for .NET](https://sourceafis.machinezoo.com/net).
        At the moment, it can benchmark algorithm accuracy, template footprint, and implementation speed.
        It also includes tools that aid in development of SourceAFIS for .NET.
        Read more on [SourceAFIS CLI](https://sourceafis.machinezoo.com/cli) page.
    '''

    def documentation_links(self):
        yield 'SourceAFIS CLI', 'https://sourceafis.machinezoo.com/cli'
        yield 'SourceAFIS for .NET', 'https://sourceafis.machinezoo.com/net'
        yield 'SourceAFIS overview', 'https://sourceafis.machinezoo.com/'

    def dependencies(self):
        yield from super().dependencies()
        yield self.use('SourceAFIS:3.14.0')
        yield self.use('System.Net.Http:4.3.4')

Project().generate()
